
using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>스캐너 필드 보정 for 2D plane (Z=0)</summary>
    public class RtcCorrection2D : ICorrection
    {
        /// <summary>bits/mm</summary>
        private float kFactor;
        /// <summary>간격 (mm)</summary>
        private float interval;
        /// <summary>SCANLAB 의 보정 유틸리티 실행파일 경로</summary>
        private readonly string exeFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "CorreXion5.exe");
        //private Rockey4ND rockey = new Rockey4ND();

        /// <summary>변환 결과에 대한 이벤트 핸들러</summary>
        public event ResultEventHandler OnResult;

        /// <summary>입력 데이타의 행 개수</summary>
        public int Rows { get; set; }

        /// <summary>입력 데이타의 열 개수</summary>
        public int Cols { get; set; }

        public float Interval => this.interval;

        public float KFactor => this.kFactor;

        /// <summary>입력 보정 파일</summary>
        public string SourceCorrectionFile { get; set; }

        /// <summary>출력 보정 파일</summary>
        public string TargetCorrectionFile { get; set; }

        /// <summary>변환 결과 로그 메시지</summary>
        public string ResultMessage { get; protected set; }

        /// <summary>절대 위치, 실측 데이타의 2차원 배열</summary>
        public CorrectionData2D[,] Data { get; set; }

        /// <summary>생성자</summary>
        /// <param name="kFactor">bits/mm</param>
        /// <param name="rows">행 개수</param>
        /// <param name="cols">열 개수</param>
        /// <param name="interval">간격</param>
        /// <param name="srcCtbFile">입력 보정 파일</param>
        /// <param name="targetCtbFile">출력 보정 파일</param>
        public RtcCorrection2D(
          float kFactor,
          int rows,
          int cols,
          float interval,
          string srcCtbFile,
          string targetCtbFile)
        {
            this.Rows = rows;
            this.Cols = cols;
            this.interval = interval;
            this.kFactor = kFactor;
            this.SourceCorrectionFile = srcCtbFile;
            this.TargetCorrectionFile = targetCtbFile;
            this.Data = new CorrectionData2D[this.Rows, this.Cols];
        }

        /// <summary>
        /// 측정 데이타 입력
        /// 좌상단부터 우상단 방향으로 순서
        /// 예 :
        /// 1 2 3
        /// 4 5 6
        /// 7 8 9
        /// </summary>
        /// <param name="row">행</param>
        /// <param name="col">열</param>
        /// <param name="reference">절대좌표</param>
        /// <param name="measured">측정좌표</param>
        /// <returns></returns>
        public bool AddAbsolute(int row, int col, Vector2 reference, Vector2 measured)
        {
            if (this.Data.Length > this.Rows * this.Cols)
                return false;
            this.Data[row, col] = new CorrectionData2D(reference, measured);
            return true;
        }

        /// <summary>
        /// 측정 데이타 입력
        /// 좌상단부터 우상단 방향으로 순서
        /// 예 :
        /// 1 2 3
        /// 4 5 6
        /// 7 8 9
        /// </summary>
        /// <param name="row">행</param>
        /// <param name="col">열</param>
        /// <param name="reference">논리적인 좌표</param>
        /// <param name="error">에러량</param>
        /// <returns></returns>
        public bool AddRelative(int row, int col, Vector2 reference, Vector2 error) => this.AddAbsolute(row, col, reference, reference + error);

        /// <summary>입력 데이타 모두 제거</summary>
        public void Clear() => this.Data = new CorrectionData2D[this.Rows, this.Cols];

        /// <summary>변환</summary>
        /// <returns></returns>
        public bool Convert()
        {
            //if (!this.rockey.Initialize() || !this.rockey.IsRtcLicensed)
            //    this.rockey.InvalidLicense();
            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", this.SourceCorrectionFile);
            if (!File.Exists(path1))
            {
                Logger.Log(Logger.Type.Error, "correction 2d input file is not founded : " + path1, Array.Empty<object>());
                return false;
            }
            string path3 = string.Format(DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".dat");
            string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", path3);
            using (StreamWriter streamWriter = new StreamWriter(path2))
            {
                streamWriter.WriteLine("[INPUT]=" + Path.GetFileNameWithoutExtension(this.SourceCorrectionFile) + " ; input table filename");
                streamWriter.WriteLine("[OUTPUT]=" + Path.GetFileNameWithoutExtension(this.TargetCorrectionFile) + " ; output table filename");
                streamWriter.WriteLine(string.Format("[CALIBRATION]={0:F8}", (object)this.kFactor));
                streamWriter.WriteLine("[RTC4]=0");
                streamWriter.WriteLine("[FITORDER]=0");
                streamWriter.WriteLine("[SMOOTHING]=-1");
                streamWriter.WriteLine("[AUTO_FIT]=0");
                streamWriter.WriteLine("[TOLERANCE]=0.1");
                streamWriter.WriteLine("[APPLY_OFFSET]= 0");
                streamWriter.WriteLine("");
                streamWriter.WriteLine("[Limit(Bits)]=524288");
                streamWriter.WriteLine(string.Format("[Limit(mm)]={0:F3}", (object)(Math.Pow(2.0, 20.0) / (double)this.kFactor)));
                streamWriter.WriteLine("[OffsetX]=0");
                streamWriter.WriteLine("[OffsetY]=0");
                streamWriter.WriteLine("[Deviation]=0");
                streamWriter.WriteLine("");
                streamWriter.WriteLine(string.Format("[GRIDNUMBERS]={0} {1}", (object)(this.Rows / 2), (object)(this.Cols / 2)));
                double num1 = (double)this.interval * (double)(this.Cols / 2);
                double num2 = (double)this.interval * (double)(this.Rows / 2);
                for (int index = 0; index < this.Cols; ++index)
                {
                    double num3 = Math.Round(-num1 + (double)this.interval * (double)index, 3);
                    streamWriter.WriteLine(string.Format("[GRIDVALUES_X]={0:F3}", (object)num3));
                }
                streamWriter.WriteLine("");
                for (int index = 0; index < this.Rows; ++index)
                {
                    double num3 = Math.Round(-num2 + (double)this.interval * (double)index, 3);
                    streamWriter.WriteLine(string.Format("[GRIDVALUES_Y]={0:F3}", (object)num3));
                }
                streamWriter.WriteLine("");
                streamWriter.WriteLine("\tXn\tYn\tX mm\tY mm");
                streamWriter.WriteLine("");
                int num4 = 0;
                for (int index1 = 0; index1 < this.Rows; ++index1)
                {
                    for (int index2 = 0; index2 < this.Cols; ++index2)
                    {
                        streamWriter.WriteLine(string.Format("\t{0}\t{1}\t{2}\t{3}", (object)(-this.Cols / 2 + index2), (object)(this.Rows / 2 - index1), (object)this.Data[index1, index2].Measured.X, (object)this.Data[index1, index2].Measured.Y));
                        ++num4;
                    }
                }
                streamWriter.WriteLine("");
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
                        Trace.Write("Timed Out");
                        Logger.Log(Logger.Type.Error, "correction 2d : " + this.exeFileName + " program timed out. failed : " + path3, Array.Empty<object>());
                        return false;
                    }
                    if (process.ExitCode != 0)
                    {
                        Trace.Write(string.Format("ExitCode : {0}", (object)process.ExitCode));
                        Logger.Log(Logger.Type.Error, string.Format("correction 2d : {0} program terminated abnormally : {1}", (object)this.exeFileName, (object)process.ExitCode), Array.Empty<object>());
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message);
                Logger.Log(Logger.Type.Error, "rtc correction 2d : fail to creating process : " + this.exeFileName + ", " + path3 + " : " + ex.Message, Array.Empty<object>());
                return false;
            }
            string path4 = path2 + ".Log";
            if (!File.Exists(path4))
            {
                Logger.Log(Logger.Type.Error, "correction 2d log file is not founded : " + path4, Array.Empty<object>());
                return false;
            }
            this.ResultMessage = string.Empty;
            this.ResultMessage = File.ReadAllText(path4);
            bool success = this.ResultMessage.Contains("written successfully");
            foreach (ResultEventHandler invocation in this.OnResult?.GetInvocationList())
                invocation((object)this, success, this.ResultMessage);
            if (success)
            {
                File.Delete(path2);
                File.Delete(path4);
            }
            if (success)
                Logger.Log(Logger.Type.Info, "success to convert correction 2d : " + this.SourceCorrectionFile + " -> " + this.TargetCorrectionFile, Array.Empty<object>());
            else
                Logger.Log(Logger.Type.Error, "fail to convert correction 2d : " + this.SourceCorrectionFile + " -> " + this.TargetCorrectionFile, Array.Empty<object>());
            return success;
        }
    }
}
