
using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>scanner correction for 3D</summary>
    public class RtcCorrection3D : ICorrection
    {
        /// <summary>bits/mm</summary>
        protected float kFactor;
        protected float interval;
        /// <summary>SCANLAB 의 보정 유틸리티 실행파일 경로</summary>
        private readonly string exeFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "stretchcorreXion5.exe");
        //private Rockey4ND rockey = new Rockey4ND();

        /// <summary>변환 결과에 대한 이벤트 핸들러</summary>
        public event ResultEventHandler OnResult;

        /// <summary>입력 데이타의 행 개수</summary>
        public int Rows { get; set; }

        /// <summary>입력 데이타의 열 개수</summary>
        public int Cols { get; set; }

        /// <summary>입력 보정 파일</summary>
        public string SourceCorrectionFile { get; set; }

        /// <summary>출력 보정 파일</summary>
        public string TargetCorrectionFile { get; set; }

        public float Interval => this.interval;

        public float KFactor => this.kFactor;

        /// <summary>Z 보정 위치 최하단값 (mm)</summary>
        public float ZLower { get; protected set; }

        /// <summary>Z 보정 위치 최상단값 (mm)</summary>
        public float ZUpper { get; protected set; }

        /// <summary>최하단의 "절대위치, 측정위치" (mm) 배열</summary>
        public CorrectionData3D[,] DataLower { get; set; }

        /// <summary>최상단의 "절대위치, 측정위치" (mm) 배열</summary>
        public CorrectionData3D[,] DataUpper { get; set; }

        /// <summary>변환 결과 로그 메시지</summary>
        public string ResultMessage { get; protected set; }

        /// <summary>생성자</summary>
        /// <param name="kFactor">bits/mm</param>
        /// <param name="rows">행 개수</param>
        /// <param name="cols">열 개수</param>
        /// <param name="interval">간격</param>
        /// <param name="zUpper">최상단 위치 (mm)</param>
        /// <param name="zLower">최하단 위치 (mm)</param>
        /// <param name="srcCtbFile">입력 보정 파일</param>
        /// <param name="targetCtbFile">출력 보정 파일</param>
        public RtcCorrection3D(
          float kFactor,
          int rows,
          int cols,
          float interval,
          float zUpper,
          float zLower,
          string srcCtbFile,
          string targetCtbFile)
        {
            this.Rows = rows;
            this.Cols = cols;
            this.interval = interval;
            this.kFactor = kFactor;
            this.ZUpper = zUpper;
            this.ZLower = zLower;
            this.kFactor = kFactor;
            this.SourceCorrectionFile = srcCtbFile;
            this.TargetCorrectionFile = targetCtbFile;
            this.DataLower = new CorrectionData3D[rows, cols];
            this.DataUpper = new CorrectionData3D[rows, cols];
        }

        /// <summary>측정 데이타 입력 (절대 좌표 값)</summary>
        /// <param name="row">행</param>
        /// <param name="col">열</param>
        /// <param name="reference">기준 좌표(mm)</param>
        /// <param name="measured">측정 절대 좌표 (mm)</param>
        /// <returns></returns>
        public bool AddAbsolute(int row, int col, Vector3 reference, Vector3 measured)
        {
            if ((double)this.ZUpper != (double)reference.Z && (double)this.ZLower != (double)reference.Z)
                return false;
            double num = ((double)this.ZUpper + (double)this.ZLower) / 2.0;
            int rows = this.Rows;
            int cols = this.Cols;
            if ((double)reference.Z > num)
            {
                this.DataUpper[row, col] = new CorrectionData3D(reference, measured);
            }
            else
            {
                if ((double)reference.Z >= num)
                    return false;
                this.DataLower[row, col] = new CorrectionData3D(reference, measured);
            }
            return true;
        }

        /// <summary>
        /// 측정 데이타 입력 (상대 좌표값)
        /// ex) 상대 좌표값 = 비전 오차량 만큼만 입력
        /// </summary>
        /// <param name="row">행</param>
        /// <param name="col">열</param>
        /// <param name="reference">기준 좌표값 (mm)</param>
        /// <param name="error">측정 상대 좌표 (mm)</param>
        /// <returns></returns>
        public bool AddRelative(int row, int col, Vector3 reference, Vector3 error) => this.AddAbsolute(row, col, reference, reference + error);

        /// <summary>입력 데이타 모두 제거</summary>
        public void Clear()
        {
            this.DataLower = new CorrectionData3D[this.Rows, this.Cols];
            this.DataUpper = new CorrectionData3D[this.Rows, this.Cols];
        }

        /// <summary>변환</summary>
        /// <returns></returns>
        public bool Convert()
        {
            //if (!this.rockey.Initialize() || !this.rockey.IsRtcLicensed)
            //    this.rockey.InvalidLicense();
            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", this.SourceCorrectionFile);
            if (!File.Exists(path1))
            {
                Logger.Log(Logger.Type.Error, "correction 3d input file is not founded : " + path1, Array.Empty<object>());
                return false;
            }
            int rows = this.Rows;
            int cols = this.Cols;
            this.ResultMessage = string.Empty;
            string path3 = string.Format(DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".dat");
            string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", path3);
            using (StreamWriter streamWriter = new StreamWriter(path2))
            {
                streamWriter.WriteLine("[INPUT]\t= " + this.SourceCorrectionFile + " ;\tinput table filename");
                streamWriter.WriteLine("[OUTPUT]\t= " + this.TargetCorrectionFile + " ;\toutput table filename");
                streamWriter.WriteLine(Environment.NewLine);
                streamWriter.WriteLine(string.Format("GRIDNUMBERS\t={0} {1} ;\tgrid x(col), y(row)", (object)(this.Cols / 2), (object)(this.Rows / 2)));
                for (int index = 0; index < this.Cols; ++index)
                {
                    double x = (double)this.DataLower[0, index].Reference.X;
                    streamWriter.WriteLine(string.Format("[GRIDVALUES_X]\t={0:F3}", (object)x));
                }
                for (int index = 0; index < this.Rows; ++index)
                {
                    double y = (double)this.DataLower[index, 0].Reference.Y;
                    streamWriter.WriteLine(string.Format("[GRIDVALUES_Y]\t={0:F3}", (object)y));
                }
                streamWriter.WriteLine(Environment.NewLine);
                streamWriter.WriteLine(string.Format("[Z_VALUE]    = {0: F3} ;\tz1(mm)", (object)this.ZUpper));
                streamWriter.WriteLine(Environment.NewLine);
                streamWriter.WriteLine("\tiX\tiY\tX[mm]\tY[mm]");
                streamWriter.WriteLine(Environment.NewLine);
                for (int index1 = 0; index1 < this.Rows; ++index1)
                {
                    for (int index2 = 0; index2 < this.Cols; ++index2)
                        streamWriter.WriteLine(string.Format("\t{0}\t{1}\t{2:F3}\t{3:F3}", (object)(-this.Cols / 2 + index2), (object)(-this.Rows / 2 + index1), (object)this.DataUpper[index1, index2].Measured.X, (object)this.DataUpper[index1, index2].Measured.Y));
                }
                streamWriter.WriteLine(Environment.NewLine);
                streamWriter.WriteLine(string.Format("[Z_VALUE] = {0: F3} ;\tz2(mm)", (object)this.ZLower));
                streamWriter.WriteLine(Environment.NewLine);
                streamWriter.WriteLine("\tiX\tiY\tX[mm]\tY[mm]");
                streamWriter.WriteLine(Environment.NewLine);
                for (int index1 = 0; index1 < this.Rows; ++index1)
                {
                    for (int index2 = 0; index2 < this.Cols; ++index2)
                        streamWriter.WriteLine(string.Format("\t{0}\t{1}\t{2:F3}\t{3:F3}", (object)(-this.Cols / 2 + index2), (object)(-this.Rows / 2 + index1), (object)this.DataLower[index1, index2].Measured.X, (object)this.DataLower[index1, index2].Measured.Y));
                }
            }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction");
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = this.exeFileName;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = path3;
            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    if (!process.WaitForExit(5000))
                    {
                        Logger.Log(Logger.Type.Error, "correction 3d : " + this.exeFileName + " program timed out. failed : " + path3, Array.Empty<object>());
                        return false;
                    }
                    int exitCode = process.ExitCode;
                    if (process.ExitCode != 0)
                    {
                        Logger.Log(Logger.Type.Error, string.Format("correction 3d : {0} program terminated abnormally : {1}", (object)this.exeFileName, (object)process.ExitCode), Array.Empty<object>());
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Type.Error, "rtc correction 3d : fail to creating process : " + this.exeFileName + ", " + path3 + " : " + ex.Message, Array.Empty<object>());
                return false;
            }
            string path4 = path2 + ".log";
            if (!File.Exists(path4))
            {
                Logger.Log(Logger.Type.Error, "rtc correction 3d : fail to open result file : " + path4, Array.Empty<object>());
                return false;
            }
            this.ResultMessage = File.ReadAllText(path4);
            bool flag = true;
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", this.TargetCorrectionFile)))
            {
                flag = false;
                Logger.Log(Logger.Type.Error, "correction 3d : fail to convert new ct5 file : " + this.exeFileName + ", " + path3, Array.Empty<object>());
            }
            bool success = flag & this.ResultMessage.Contains("STRETCH_X");
            foreach (ResultEventHandler invocation in this.OnResult?.GetInvocationList())
                invocation((object)this, success, this.ResultMessage);
            if (success)
                Logger.Log(Logger.Type.Info, "success to convert correction 3d : " + this.SourceCorrectionFile + " -> " + this.TargetCorrectionFile, Array.Empty<object>());
            else
                Logger.Log(Logger.Type.Error, "fail to convert correction 3d : " + this.SourceCorrectionFile + " -> " + this.TargetCorrectionFile, Array.Empty<object>());
            return true;
        }
    }
}
