// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Rtc4
// Assembly: spirallab.sirius.rtc, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 330B13B0-CD9B-4679-A17E-EBB26CA3FE4F
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.rtc.dll

using RTC4Import;
using System;
using System.IO;
using System.Numerics;
using System.Threading;

namespace Scanlab.Sirius
{
    /// <summary>RTC4 객체</summary>
    public class Rtc4 : IRtc, IDisposable, IRtcExtension, IRtcMOTF
    {
        /// <summary>카드 개수</summary>
        public static uint Count;
        private string[] ctbFileName = new string[2];
        /// <summary>물리적인 마지막 위치값</summary>
        protected Vector3 vPhysical3D;
        /// <summary>논리적인 마지막 위치값</summary>
        protected Vector3 vLogical3D;
        /// <summary>엔코더 X 의 가상 속도 mm/s</summary>
        private float EncXSimulatedSpeed;
        /// <summary>엔코더 Y 의 가상 속도 mm/s</summary>
        private float EncYSimulatedSpeed;
        protected bool isAborted;
        protected ushort listIndex;
        protected uint listTempCount;
        protected uint listTotalCount;
        protected bool extCtrlMode;
        protected readonly uint RTC4_LIST_BUFFER_MAX = 3500;
        protected Vector2 vPhysical;
        protected Vector2 vLogical;
        protected string outputFileName;
        protected StreamWriter stream;
        protected float frequency;
        protected float pulseWidth;
        protected uint valueByVector;
        protected ListType listType;
        protected bool isManualOn;
        private bool disposed;
        //private Rockey4ND rockey = new Rockey4ND();
        private ExtensionChannel pixelExtensionChannel;

        /// <summary>RTC 제어기 식별 번호 (1,2,3,...)</summary>
        public uint Index { get; set; }

        /// <summary>이름</summary>
        public string Name { get; set; }

        /// <summary>행렬 스택</summary>
        public IMatrixStack MatrixStack { get; set; }

        /// <summary>bits/mm 값 (2^16)</summary>
        public float KFactor { get; set; }

        /// <summary>
        /// 레이저 모드(CO2, Yag1,2,3,4, ...)
        /// Laser1,2 출력핀의 타이밍 종류 선택
        /// </summary>
        public LaserMode LaserMode { get; protected set; }

        /// <summary>First Pulse Killer 신호의 시간값 (usec)</summary>
        public float Fpk { get; protected set; }

        /// <summary>MOTF 옵션 여부</summary>
        public bool IsMOTF { get; protected set; }

        /// <summary>듀얼 헤드 옵션 여부</summary>
        public bool Is2ndHead { get; protected set; }

        /// <summary>3D 옵션 여부</summary>
        public bool Is3D { get; protected set; }

        /// <summary>Scan Ahead 옵션 여부</summary>
        public bool IsScanAhead { get; protected set; }

        /// <summary>UFPM(Ultra Fast Pulse Modulation) 옵션 여부</summary>
        public bool IsUFPM { get; protected set; }

        /// <summary>SyncAxis (XLScan) 옵션 여부</summary>
        public bool IsSyncAxis { get; protected set; }

        /// <summary>스캐너 보정 파일 목록 (최대 2개 로드 가능)</summary>
        public string[] CorrectionFiles => this.ctbFileName;

        /// <summary>첫번째 스캐너 헤드 보정 테이블 번호 (1~2)</summary>
        public CorrectionTableIndex PrimaryHeadTable { get; protected set; }

        /// <summary>두번째 스캐너 헤드 보정 테이블 번호 (1~2)</summary>
        public CorrectionTableIndex SecondaryHeadTable { get; protected set; }

        /// <summary>주 스캐너 헤드 X,Y 오프셋 (mm)</summary>
        public Vector2 PrimaryHeadOffset { get; protected set; }

        /// <summary>주 스캔 헤드 회전 (각도)</summary>
        public float PrimaryHeadAngle { get; protected set; }

        /// <summary>2nd 스캐너 헤드 X,Y 오프셋 (mm)</summary>
        public Vector2 SecondaryHeadOffset { get; protected set; }

        /// <summary>2nd 스캔 헤드 회전 (각도)</summary>
        public float SecondaryHeadAngle { get; protected set; }

        /// <summary>Z 이동량 오프셋 (mm)</summary>
        public float ZOffset { get; protected set; }

        /// <summary>Z 디포커스 값 (mm)</summary>
        public float ZDefocus { get; protected set; }

        /// <summary>Z 축 bits/mm 값</summary>
        public float KZFactor { get; protected set; }

        /// <summary>단위 mm 당 엔코더 X 개수</summary>
        public int EncXCountsPerMm { get; set; }

        /// <summary>단위 mm 당 엔코더 Y 개수</summary>
        public int EncYCountsPerMm { get; set; }

        /// <summary>생성자</summary>
        public Rtc4()
        {
            this.Index = 0U;
            this.outputFileName = string.Empty;
            this.isAborted = false;
            this.listIndex = (ushort)1;
            this.listTempCount = 0U;
            this.listTotalCount = 0U;
            this.extCtrlMode = false;
            this.MatrixStack = (IMatrixStack)new Scanlab.Sirius.MatrixStack();
            this.vPhysical = Vector2.Zero;
            this.vLogical = Vector2.Zero;
            this.stream = (StreamWriter)null;
            this.frequency = this.pulseWidth = 0.0f;
            this.listType = ListType.Single;
            this.valueByVector = 0U;
            this.vPhysical3D = Vector3.Zero;
            this.vLogical3D = Vector3.Zero;
            this.PrimaryHeadOffset = Vector2.Zero;
            this.PrimaryHeadAngle = 0.0f;
            this.SecondaryHeadOffset = Vector2.Zero;
            this.SecondaryHeadAngle = 0.0f;
            this.ZOffset = this.ZDefocus = 0.0f;
            this.EncXCountsPerMm = this.EncYCountsPerMm = 0;
            this.EncXSimulatedSpeed = this.EncYSimulatedSpeed = 0.0f;
            ++Rtc4.Count;
        }

        /// <summary>생성자</summary>
        /// <param name="index">카드번호 (0,1,2,...)</param>
        /// <param name="outputFileName">리스트 명령 로그 출력 파일 이름</param>
        public Rtc4(uint index, string outputFileName = "")
          : this()
        {
            this.Index = index;
            this.outputFileName = outputFileName;
        }

        /// <summary>종결자</summary>
        ~Rtc4()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }

        /// <summary>자원 해제 - IDisposable 인터페이스 구현</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
                --Rtc4.Count;
            this.disposed = true;
        }

        /// <summary>RTC 카드 초기화</summary>
        /// <summary>RTC 카드 초기화</summary>
        /// <param name="kFactor">k factor = bits/mm</param>
        /// <param name="laserMode">LaserMode 열거형</param>
        /// <param name="ctbFileName">주 스캐너의 보정 테이블(Table1)에 Load/Select 하려는 .ct5 스캐너 보정 파일</param>
        /// <returns></returns>
        public virtual bool Initialize(float kFactor, LaserMode laserMode, string ctbFileName)
        {
            //if (!this.rockey.Initialize() || !this.rockey.IsRtcLicensed)
            //    this.rockey.InvalidLicense();
            this.KFactor = kFactor;
            short num1 = RTC4Wrap.n_load_program_file((ushort)(this.Index + 1U), "RTC4D2.hex");
            if (num1 != (short)0)
            {
                Logger.Log(Logger.Type.Error, string.Format("rtc4 [{0}]: fail to load_program_file = {1}", (object)this.Index, (object)num1), Array.Empty<object>());
                return false;
            }
            int num2 = (int)RTC4Wrap.rtc4_count_cards();
            int num3 = (int)RTC4Wrap.get_dll_version();
            int num4 = (int)RTC4Wrap.get_hex_version();
            uint num5 = (uint)RTC4Wrap.get_rtc_version();
            uint num6 = (uint)RTC4Wrap.n_get_serial_number((ushort)(this.Index + 1U));
            this.IsMOTF = Convert.ToBoolean(num5 & 256U);
            this.Is2ndHead = Convert.ToBoolean(num5 & 512U);
            this.Is3D = Convert.ToBoolean(num5 & 1024U);
            if (this.Is3D)
            {
                short num7 = RTC4Wrap.n_load_program_file((ushort)(this.Index + 1U), "RTC4D3.hex");
                if (num7 != (short)0)
                {
                    Logger.Log(Logger.Type.Error, string.Format("rtc4 [{0}]: fail to load_program_file = {1}", (object)this.Index, (object)num7), Array.Empty<object>());
                    return false;
                }
            }
            if (!this.CtlLoadCorrectionFile(CorrectionTableIndex.Table1, ctbFileName))
                return false;
            if (!this.Is3D)
            {
                if (!this.CtlSelectCorrection(CorrectionTableIndex.Table1, CorrectionTableIndex.None))
                    return false;
            }
            else if (!this.CtlSelectCorrection(CorrectionTableIndex.Table1, CorrectionTableIndex.Table1))
                return false;
            this.KZFactor = kFactor;
            this.LaserMode = laserMode;
            this.Fpk = 0.0f;
            RTC4Wrap.n_set_laser_mode((ushort)(this.Index + 1U), (ushort)laserMode);
            RTC4Wrap.n_set_firstpulse_killer((ushort)(this.Index + 1U), (ushort)0);
            RTC4Wrap.n_set_standby((ushort)(this.Index + 1U), (ushort)0, (ushort)0);
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: serial no = {1} initialized", (object)this.Index, (object)num6), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 보정 파일(.ctb)을 RTC 내부 메모리로 로딩
        /// RTC4의 경우 2개의 버퍼 테이블만 사용 가능
        /// </summary>
        /// <param name="tableIndex">CorrectionTableIndex 열거형 </param>
        /// <param name="ctbFileName">.ctb 스캐너 보정 파일</param>
        /// <returns></returns>
        public bool CtlLoadCorrectionFile(CorrectionTableIndex tableIndex, string ctbFileName)
        {
            if (this.CtlGetStatus(RtcStatus.Busy) || RTC4Wrap.n_load_correction_file((ushort)(this.Index + 1U), ctbFileName, (short)(tableIndex + 1), 1.0, 1.0, 0.0, 0.0, 0.0) != (short)0)
                return false;
            this.ctbFileName[(int)tableIndex] = ctbFileName;
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: correction file loaded : {1} at {2}", (object)this.Index, (object)ctbFileName, (object)tableIndex.ToString()), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 지정된 스캐너 헤드에 보정 파일을 설정
        /// RTC4의 경우 2개의 버퍼 테이블만 사용 가능
        /// </summary>
        /// <param name="primaryHeadTableIndex">CorrectionTableIndex 열거형 (Primary)</param>
        /// <param name="secondaryHeadTableIndex">CorrectionTableIndex 열거형 (Secondary)</param>
        /// <returns></returns>
        public bool CtlSelectCorrection(
          CorrectionTableIndex primaryHeadTableIndex,
          CorrectionTableIndex secondaryHeadTableIndex = CorrectionTableIndex.None)
        {
            if (!this.Is3D)
            {
                RTC4Wrap.n_select_cor_table((ushort)(this.Index + 1U), (ushort)(primaryHeadTableIndex + 1), (ushort)(secondaryHeadTableIndex + 1));
                this.PrimaryHeadTable = primaryHeadTableIndex;
                this.SecondaryHeadTable = secondaryHeadTableIndex;
            }
            else
            {
                RTC4Wrap.n_select_cor_table((ushort)(this.Index + 1U), (ushort)(primaryHeadTableIndex + 1), (ushort)(secondaryHeadTableIndex + 1));
                this.PrimaryHeadTable = primaryHeadTableIndex;
                this.SecondaryHeadTable = secondaryHeadTableIndex;
            }
            // ISSUE: variable of a boxed type
            uint index = this.Index;
            CorrectionTableIndex correctionTableIndex = this.PrimaryHeadTable;
            string str1 = correctionTableIndex.ToString();
            correctionTableIndex = this.SecondaryHeadTable;
            string str2 = correctionTableIndex.ToString();
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: correction file selected : primary head= {1}. secondary head= {2}", (object)index, (object)str1, (object)str2), Array.Empty<object>());
            return true;
        }

        /// <summary>현재 설정된 주파수, 펄스폭 으로 레이저 변조 신호(LASER1,2,ON) 출력</summary>
        /// <returns></returns>
        public bool CtlLaserOn()
        {
            RTC4Wrap.n_laser_signal_on((ushort)(this.Index + 1U));
            this.isManualOn = true;
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: laser is on by manually", (object)this.Index), Array.Empty<object>());
            return true;
        }

        /// <summary>레이저 변호 신호 (LASER1,2,ON) 중단</summary>
        /// <returns></returns>
        public bool CtlLaserOff()
        {
            RTC4Wrap.n_laser_signal_off((ushort)(this.Index + 1U));
            this.isManualOn = false;
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: laser is off by manually", (object)this.Index), Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 위치로 스캐너 수동 이동</summary>
        /// <param name="vPosition">X,Y (mm)</param>
        /// <returns></returns>
        public bool CtlMove(Vector2 vPosition)
        {
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            int num1 = (int)(short)((double)vector2.X * (double)this.KFactor);
            int num2 = (int)(short)((double)vector2.Y * (double)this.KFactor);
            RTC4Wrap.n_goto_xy((ushort)(this.Index + 1U), (short)num1, (short)num2);
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: scanner moved : {1:F3}, {2:F3}", (object)this.Index, (object)vector2.X, (object)vector2.Y), Array.Empty<object>());
            this.vPhysical = vector2;
            this.vLogical = vPosition;
            return true;
        }

        /// <summary>지정된 위치로 스캐너 수동 이동</summary>
        /// <param name="x">x mm</param>
        /// <param name="y">y mm</param>
        /// <returns></returns>
        public bool CtlMove(float x, float y) => this.CtlMove(new Vector2(x, y));

        /// <summary>주파수와 펄스폭 설정</summary>
        /// <param name="frequency">주파수 (Hz)</param>
        /// <param name="pulseWidth">펄스폭 (usec)</param>
        /// <returns></returns>
        public bool CtlFrequency(float frequency, float pulseWidth)
        {
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            double num1 = 1.0 / (double)frequency * 1000000.0;
            double num2 = num1 / 2.0;
            RTC4Wrap.n_set_start_list((ushort)(this.Index + 1U), (ushort)1);
            if (num1 < 2.0)
                RTC4Wrap.n_set_laser_timing((ushort)(this.Index + 1U), (ushort)(num2 * 8.0), (ushort)((double)pulseWidth * 8.0), (ushort)((double)pulseWidth * 8.0), (ushort)1);
            else
                RTC4Wrap.n_set_laser_timing((ushort)(this.Index + 1U), (ushort)(num2 * 8.0), (ushort)pulseWidth, (ushort)pulseWidth, (ushort)0);
            RTC4Wrap.n_set_end_of_list((ushort)(this.Index + 1U));
            RTC4Wrap.n_execute_list((ushort)(this.Index + 1U), (ushort)1);
            this.frequency = frequency;
            this.pulseWidth = pulseWidth;
            this.CtlBusyWait();
            return true;
        }

        /// <summary>스캐너/ 레이저 지연값 설정</summary>
        /// <param name="laserOn">레이저 온 지연 (usec)</param>
        /// <param name="laserOff">레이저 오프 지연 (usec)</param>
        /// <param name="scannerJump">스캐너 점프 지연 (usec)</param>
        /// <param name="scannerMark">스캐너 마크 지연 (usec)</param>
        /// <param name="scannerPolygon">스캐너 폴리곤(코너) 지연 (usec)</param>
        /// <returns></returns>
        public bool CtlDelay(
          float laserOn,
          float laserOff,
          float scannerJump,
          float scannerMark,
          float scannerPolygon)
        {
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            RTC4Wrap.n_set_start_list((ushort)(this.Index + 1U), (ushort)1);
            RTC4Wrap.n_set_scanner_delays((ushort)(this.Index + 1U), (ushort)((double)scannerJump / 10.0), (ushort)((double)scannerMark / 10.0), (ushort)((double)scannerPolygon / 10.0));
            RTC4Wrap.n_set_laser_delays((ushort)(this.Index + 1U), (short)laserOn, (short)laserOff);
            RTC4Wrap.n_set_end_of_list((ushort)(this.Index + 1U));
            RTC4Wrap.n_execute_list((ushort)(this.Index + 1U), (ushort)1);
            this.CtlBusyWait();
            return true;
        }

        /// <summary>스캐너 점프/마크 속도 설정</summary>
        /// <param name="jump">점프(jump) 속도 (mm/s)</param>
        /// <param name="mark">마크(mark) 및 아크(arc) 속도 (mm/s)</param>
        /// <returns></returns>
        public bool CtlSpeed(float jump, float mark)
        {
            if (this.CtlGetStatus(RtcStatus.Busy) || this.CtlGetStatus(RtcStatus.Busy))
                return false;
            RTC4Wrap.n_set_start_list((ushort)(this.Index + 1U), (ushort)1);
            double speed1 = (double)jump / 1000.0 * (double)this.KFactor;
            double speed2 = (double)mark / 1000.0 * (double)this.KFactor;
            if (speed1 < 1.0)
                speed1 = 1.0;
            if (speed1 > 50000.0)
                speed1 = 50000.0;
            if (speed2 < 1.0)
                speed2 = 1.0;
            if (speed2 > 50000.0)
                speed2 = 50000.0;
            RTC4Wrap.n_set_jump_speed((ushort)(this.Index + 1U), speed1);
            RTC4Wrap.n_set_mark_speed((ushort)(this.Index + 1U), speed2);
            RTC4Wrap.n_set_end_of_list((ushort)(this.Index + 1U));
            RTC4Wrap.n_execute_list((ushort)(this.Index + 1U), (ushort)1);
            this.CtlBusyWait();
            return true;
        }

        /// <summary>확장 포트에 데이타 쓰기</summary>
        /// <typeparam name="T">값(16비트, 8비트, 2비트 (uint), 아나로그(float 10V)</typeparam>
        /// <param name="ch">확장 커넥터 종류 </param>
        /// <param name="value">uint/float</param>
        /// <param name="compensator">compensator 보정용 객체</param>
        /// <returns></returns>
        public bool CtlWriteData<T>(ExtensionChannel ch, T value, ICompensator<T> compensator = null)
        {
            if (compensator != null)
            {
                T output;
                if (!compensator.Interpolate(value, out output))
                    return false;
                value = output;
            }
            switch (ch)
            {
                case ExtensionChannel.ExtDO2:
                    int num1 = (int)(uint)Convert.ChangeType((object)value, typeof(uint));
                    break;
                case ExtensionChannel.ExtDO8:
                    ushort num2 = (ushort)Convert.ChangeType((object)value, typeof(uint));
                    RTC4Wrap.n_write_8bit_port((ushort)(this.Index + 1U), num2);
                    break;
                case ExtensionChannel.ExtDO16:
                    ushort num3 = (ushort)Convert.ChangeType((object)value, typeof(uint));
                    RTC4Wrap.n_write_io_port((ushort)(this.Index + 1U), num3);
                    break;
                case ExtensionChannel.ExtAO1:
                    float num4 = (float)Convert.ChangeType((object)value, typeof(float));
                    ushort num5 = (ushort)((Math.Pow(2.0, 10.0) - 1.0) * (double)num4 / 10.0);
                    RTC4Wrap.n_write_da_x((ushort)(this.Index + 1U), (ushort)1, num5);
                    break;
                case ExtensionChannel.ExtAO2:
                    float num6 = (float)Convert.ChangeType((object)value, typeof(float));
                    ushort num7 = (ushort)((Math.Pow(2.0, 10.0) - 1.0) * (double)num6 / 10.0);
                    RTC4Wrap.n_write_da_x((ushort)(this.Index + 1U), (ushort)2, num7);
                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>확장1 포트의 16비트 디지털 출력의 특정 비트값을 변경</summary>
        /// <param name="bitPosition">0~15</param>
        /// <param name="onOff">출력</param>
        /// <returns></returns>
        public bool CtlWriteExtDO16(ushort bitPosition, bool onOff) => false;

        /// <summary>확장 포트에서 데이타 읽기</summary>
        /// <typeparam name="T">값(16비트, 8비트, 2비트 (uint), 아나로그(float 10V)</typeparam>
        /// <param name="ch">확장 커넥터 종류</param>
        /// <param name="value">uint/float</param>
        /// <param name="compensator">compensator 보정용 객체</param>
        /// <returns></returns>
        public bool CtlReadData<T>(ExtensionChannel ch, out T value, ICompensator<T> compensator = null)
        {
            value = default(T);
            switch (ch)
            {
                case ExtensionChannel.ExtDO16:
                    uint num1 = (uint)RTC4Wrap.n_get_io_status((ushort)(this.Index + 1U));
                    value = (T)Convert.ChangeType((object)num1, typeof(T));
                    break;
                case ExtensionChannel.ExtDI16:
                    uint num2 = (uint)RTC4Wrap.n_read_io_port((ushort)(this.Index + 1U));
                    value = (T)Convert.ChangeType((object)num2, typeof(T));
                    break;
                default:
                    return false;
            }
            if (compensator != null)
            {
                T output;
                if (!compensator.Interpolate(value, out output))
                    return false;
                value = output;
            }
            return true;
        }

        /// <summary>RTC 카드의 상태 확인</summary>
        /// <param name="status">RtcStatus 열거형</param>
        /// <returns></returns>
        public bool CtlGetStatus(RtcStatus status)
        {
            bool flag = false;
            switch (status)
            {
                case RtcStatus.Busy:
                    ushort busy;
                    ushort position;
                    RTC4Wrap.n_get_status((ushort)(this.Index + 1U), out busy, out position);
                    flag = flag | busy > (ushort)0 | this.isManualOn;
                    break;
                case RtcStatus.NotBusy:
                    flag = !this.CtlGetStatus(RtcStatus.Busy);
                    break;
                case RtcStatus.List1Busy:
                    flag = Convert.ToBoolean((uint)RTC4Wrap.n_read_status((ushort)(this.Index + 1U)) & 15U);
                    break;
                case RtcStatus.List2Busy:
                    flag = Convert.ToBoolean((uint)RTC4Wrap.n_read_status((ushort)(this.Index + 1U)) & 16U);
                    break;
                case RtcStatus.NoError:
                    flag = !this.CtlGetStatus(RtcStatus.Aborted);
                    break;
                case RtcStatus.Aborted:
                    flag = this.isAborted;
                    break;
                case RtcStatus.PositionAckOK:
                    int num = (int)RTC4Wrap.n_get_head_status((ushort)(this.Index + 1U), (ushort)1);
                    flag = Convert.ToBoolean((uint)(num & 8)) & Convert.ToBoolean((uint)(num & 16));
                    break;
                case RtcStatus.PowerOK:
                    flag = Convert.ToBoolean((uint)RTC4Wrap.n_get_head_status((ushort)(this.Index + 1U), (ushort)1) & 128U);
                    break;
                case RtcStatus.TempOK:
                    flag = Convert.ToBoolean((uint)RTC4Wrap.n_get_head_status((ushort)(this.Index + 1U), (ushort)1) & 64U);
                    break;
            }
            return flag;
        }

        /// <summary>리스트 명령이 완료될 때(busy 가 해제될때) 까지 대기하는 함수</summary>
        /// <returns></returns>
        public bool CtlBusyWait()
        {
            ushort busy;
            do
            {
                Thread.Sleep(10);
                ushort position;
                RTC4Wrap.n_get_status((ushort)(this.Index + 1U), out busy, out position);
            }
            while (busy != (ushort)0);
            return true;
        }

        /// <summary>실행중인 리스트 명령(busy 상태를)을 강제 종료</summary>
        /// <returns></returns>
        public bool CtlAbort()
        {
            RTC4Wrap.n_stop_execution((ushort)(this.Index + 1U));
            this.isAborted = true;
            this.stream?.Flush();
            this.stream?.Dispose();
            this.stream = (StreamWriter)null;
            Logger.Log(Logger.Type.Warn, string.Format("rtc4 [{0}]: trying to abort !", (object)this.Index), Array.Empty<object>());
            return this.CtlGetStatus(RtcStatus.NotBusy);
        }

        /// <summary>에러상태를 해제</summary>
        /// <returns></returns>
        public bool CtlReset()
        {
            this.isAborted = false;
            return true;
        }

        /// <summary>리스트 명령 시작 - 버퍼 준비</summary>
        /// <param name="laser">레이저 소스</param>
        /// <param name="listType">리스트 타입 (하나의 거대한 리스트 : single, 더블 버퍼링되는 두개의 리스트 : double)</param>
        /// <returns></returns>
        public bool ListBegin(ILaser laser, ListType listType = ListType.Single)
        {
            //if (!this.rockey.IsRtcLicensed)
            //    this.rockey.InvalidLicense();
            if (!string.IsNullOrEmpty(this.outputFileName))
            {
                this.stream?.Dispose();
                this.stream = new StreamWriter(this.outputFileName);
            }
            this.listTempCount = 0U;
            this.listTotalCount = 0U;
            this.isAborted = false;
            this.listType = listType;
            this.listIndex = (ushort)1;
            if (this.listType != ListType.Single)
                ;
            RTC4Wrap.n_set_start_list((ushort)(this.Index + 1U), this.listIndex);
            this.stream?.WriteLine("; LIST HAS BEGAN : " + DateTime.Now.ToString() + " with " + listType.ToString());
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: list has began ...", (object)this.Index), Array.Empty<object>());
            return true;
        }

        /// <summary>리스트 명령 - 주파수, 펄스폭</summary>
        /// <param name="frequency">주파수 (Hz)</param>
        /// <param name="pulseWidth">펄스폭 (usec)</param>
        /// <returns></returns>
        public bool ListFrequency(float frequency, float pulseWidth)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            double num = 1.0 / (double)frequency * 1000000.0 / 2.0;
            if (!this.IsListReady(1U))
                return false;
            if (num < 1.0)
                RTC4Wrap.n_set_laser_timing((ushort)(this.Index + 1U), (ushort)(num * 8.0), (ushort)((double)pulseWidth * 8.0), (ushort)((double)pulseWidth * 8.0), (ushort)1);
            else
                RTC4Wrap.n_set_laser_timing((ushort)(this.Index + 1U), (ushort)num, (ushort)pulseWidth, (ushort)pulseWidth, (ushort)0);
            this.frequency = frequency;
            this.pulseWidth = pulseWidth;
            this.stream?.WriteLine(string.Format("FREQUENCY_HZ = {0:F3}", (object)frequency));
            this.stream?.WriteLine(string.Format("PULSE_WIDTH_US = {0:F3}", (object)pulseWidth));
            return true;
        }

        /// <summary>리스트 명령 - 지연</summary>
        /// <param name="laserOn">레이저 온 지연 (usec)</param>
        /// <param name="laserOff">레이저 오프 지연 (usec)</param>
        /// <param name="scannerJump">스캐너 점프 지연 (usec)</param>
        /// <param name="scannerMark">스캐너 마크 지연 (usec)</param>
        /// <param name="scannerPolygon">스캐너 폴리곤(코너) 지연 (usec)</param>
        /// <returns></returns>
        public bool ListDelay(
          float laserOn,
          float laserOff,
          float scannerJump,
          float scannerMark,
          float scannerPolygon)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(2U))
                return false;
            RTC4Wrap.n_set_scanner_delays((ushort)(this.Index + 1U), (ushort)((double)scannerJump / 10.0), (ushort)((double)scannerMark / 10.0), (ushort)((double)scannerPolygon / 10.0));
            RTC4Wrap.n_set_laser_delays((ushort)(this.Index + 1U), (short)laserOn, (short)laserOff);
            this.stream?.WriteLine(string.Format("LASER_ON_DELAY_US = {0:F3}", (object)laserOn));
            this.stream?.WriteLine(string.Format("LASER_OFF_DELAY_US = {0:F3}", (object)laserOff));
            this.stream?.WriteLine(string.Format("SCANNER_JUMP_DELAY_US = {0:F3}", (object)scannerJump));
            this.stream?.WriteLine(string.Format("SCANNER_MARK_DELAY_US = {0:F3}", (object)scannerMark));
            this.stream?.WriteLine(string.Format("SCANNER_POLYGON_DELAY_US = {0:F3}", (object)scannerPolygon));
            return true;
        }

        /// <summary>리스트 명령 - 속도</summary>
        /// <param name="jump">점프(jump 속도 (mm/s)</param>
        /// <param name="mark">마크(mark/arc) 속도 (mm/s)</param>
        /// <returns></returns>
        public bool ListSpeed(float jump, float mark)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            double speed1 = (double)jump / 1000.0 * (double)this.KFactor;
            double speed2 = (double)mark / 1000.0 * (double)this.KFactor;
            if (speed1 < 1.0)
                speed1 = 1.0;
            if (speed1 > 50000.0)
                speed1 = 50000.0;
            if (speed2 < 1.0)
                speed2 = 1.0;
            if (speed2 > 50000.0)
                speed2 = 50000.0;
            if (!this.IsListReady(2U))
                return false;
            RTC4Wrap.n_set_jump_speed((ushort)(this.Index + 1U), speed1);
            RTC4Wrap.n_set_mark_speed((ushort)(this.Index + 1U), speed2);
            this.stream?.WriteLine(string.Format("SCANNER_JUMP_SPEED_MM_S = {0:F3}", (object)jump));
            this.stream?.WriteLine(string.Format("SCANNER_MARK_SPEED_MM_S = {0:F3}", (object)mark));
            return true;
        }

        /// <summary>리스트 명령 - 시간 대기</summary>
        /// <param name="msec">시간 (msec)</param>
        /// <returns></returns>
        public bool ListWait(float msec)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if ((double)msec * 1000.0 >= 10.0)
            {
                for (; (double)msec > 500.0; msec -= 500f)
                {
                    if (!this.IsListReady(1U))
                        return false;
                    RTC4Wrap.n_long_delay((ushort)(this.Index + 1U), (ushort)50000);
                }
                if ((double)msec > 0.0)
                {
                    if (!this.IsListReady(1U))
                        return false;
                    RTC4Wrap.n_long_delay((ushort)(this.Index + 1U), (ushort)((double)msec * 100.0));
                }
            }
            this.stream?.WriteLine(string.Format("WAIT_DURING_MS = {0:F6}", (object)msec));
            return true;
        }

        /// <summary>리스트 명령 - 레이저 출사 시간</summary>
        /// <param name="msec">시간 (msec)</param>
        /// <returns></returns>
        public bool ListLaserOn(float msec)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            double num;
            for (num = (double)msec; num > 500.0; num -= 500.0)
            {
                if (!this.IsListReady(1U))
                    return false;
                RTC4Wrap.n_laser_on_list((ushort)(this.Index + 1U), (ushort)50000);
            }
            if (!this.IsListReady(1U))
                return false;
            RTC4Wrap.n_laser_on_list((ushort)(this.Index + 1U), (ushort)(num * 1000.0 / 10.0));
            this.stream?.WriteLine(string.Format("LASER_ON_DURING_MS = {0:F6}", (object)msec));
            return true;
        }

        /// <summary>리스트 명령 - 레이저 출사 시작</summary>
        /// <returns></returns>
        public bool ListLaserOn()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            RTC4Wrap.n_laser_signal_on_list((ushort)(this.Index + 1U));
            this.stream?.WriteLine("LASER_ON");
            return true;
        }

        /// <summary>리스트 명령 - 레이저 출사 중지</summary>
        /// <returns></returns>
        public bool ListLaserOff()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            RTC4Wrap.n_laser_signal_off_list((ushort)(this.Index + 1U));
            this.stream?.WriteLine("LASER_OFF");
            return true;
        }

        /// <summary>리스트 명령 - 점프</summary>
        /// <param name="vPosition">x,y 위치 (mm)</param>
        /// <param name="rampFactor">ALC(Automatic Laser Control) 사용시 비율값</param>
        /// <returns></returns>
        public bool ListJump(Vector2 vPosition, float rampFactor = 1f)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if (this.IsDuplicated(vPosition))
                return true;
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            int num1 = (int)((double)vector2.X * (double)this.KFactor);
            int num2 = (int)((double)vector2.Y * (double)this.KFactor);
            if (!this.IsListReady(1U))
                return false;
            RTC4Wrap.n_jump_abs((ushort)(this.Index + 1U), (short)num1, (short)num2);
            this.stream?.WriteLine(string.Format("JUMP_TO = {0:F3}, {1:F3}", (object)vector2.X, (object)vector2.Y));
            this.vPhysical = vector2;
            this.vLogical = vPosition;
            return true;
        }

        /// <summary>리스트 명령 - 점프</summary>
        /// <param name="x">x 위치 (mm)</param>
        /// <param name="y">y 위치 (mm)</param>
        /// <param name="rampFactor">ALC(Automatic Laser Control) 사용시 비율값</param>
        /// <returns></returns>
        public bool ListJump(float x, float y, float rampFactor = 1f) => this.ListJump(new Vector2(x, y), rampFactor);

        /// <summary>리스트 명령 - 마크 (Mark : 선분)</summary>
        /// <param name="vPosition">x,y 위치 (mm)</param>
        /// <param name="rampFactor">ALC(Automatic Laser Control) 사용시 비율값</param>
        /// <returns></returns>
        public bool ListMark(Vector2 vPosition, float rampFactor = 1f)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if (this.IsDuplicated(vPosition))
                return true;
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            int num1 = (int)((double)vector2.X * (double)this.KFactor);
            int num2 = (int)((double)vector2.Y * (double)this.KFactor);
            if (!this.IsListReady(1U))
                return false;
            RTC4Wrap.n_mark_abs((ushort)(this.Index + 1U), (short)num1, (short)num2);
            this.stream?.WriteLine(string.Format("MARK_TO = {0:F3}, {1:F3}", (object)vector2.X, (object)vector2.Y));
            this.vPhysical = vector2;
            this.vLogical = vPosition;
            return true;
        }

        /// <summary>리스트 명령 - 마크 (Mark : 선분)</summary>
        /// <param name="x">x 위치 (mm)</param>
        /// <param name="y">y 위치 (mm)</param>
        /// <param name="rampFactor">ALC(Automatic Laser Control) 사용시 비율값</param>
        /// <returns></returns>
        public bool ListMark(float x, float y, float rampFactor = 1f) => this.ListMark(new Vector2(x, y), rampFactor);

        /// <summary>리스트 명령 - 아크 (Arc : 호)</summary>
        /// <param name="vCenter">회전 중심 위치 (cx, cy)</param>
        /// <param name="sweepAngle">회전량 (+ : CCW, - : CW)</param>
        /// <returns></returns>
        public bool ListArc(Vector2 vCenter, float sweepAngle)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            Vector2 vector2 = Vector2.Transform(vCenter, this.MatrixStack.ToResult);
            int num1 = (int)((double)Math.Abs(sweepAngle) / 360.0);
            double num2 = (double)sweepAngle - (double)Math.Sign(sweepAngle) * 360.0 * (double)num1;
            if (!this.IsListReady((uint)(num1 + 1)))
                return false;
            for (int index = 0; index < num1; ++index)
                RTC4Wrap.n_arc_abs((ushort)(this.Index + 1U), (short)((double)vector2.X * (double)this.KFactor), (short)((double)vector2.Y * (double)this.KFactor), (double)Math.Sign(sweepAngle) * -360.0);
            RTC4Wrap.n_arc_abs((ushort)(this.Index + 1U), (short)((double)vector2.X * (double)this.KFactor), (short)((double)vector2.Y * (double)this.KFactor), -num2);
            this.stream?.WriteLine(string.Format("ARC_BY_CENTER = {0:F3}, {1:F3}, SWEEP_ANGLE = {2:F3}", (object)vector2.X, (object)vector2.Y, (object)sweepAngle));
            double num3 = 0.0;
            if ((double)this.vLogical.Y != (double)vCenter.Y || (double)this.vLogical.X != (double)vCenter.Y)
                num3 = Math.Atan2((double)this.vLogical.Y - (double)vCenter.Y, (double)this.vLogical.X - (double)vCenter.X);
            double num4 = Math.Sqrt(((double)vCenter.X - (double)this.vLogical.X) * ((double)vCenter.X - (double)this.vLogical.X) + ((double)vCenter.Y - (double)this.vLogical.Y) * ((double)vCenter.Y - (double)this.vLogical.Y));
            this.vLogical.X = (float)(num4 * Math.Cos(num3 + Math.PI / 180.0 * (double)sweepAngle)) + vCenter.X;
            this.vLogical.Y = (float)(num4 * Math.Sin(num3 + Math.PI / 180.0 * (double)sweepAngle)) + vCenter.Y;
            this.vPhysical = Vector2.Transform(this.vLogical, this.MatrixStack.ToResult);
            return true;
        }

        /// <summary>리스트 명령 - 아크 (Arc : 호)</summary>
        /// <param name="cx">회전 중심 위치 (cx)</param>
        /// <param name="cy">회전 중심 위치 (cy)</param>
        /// <param name="sweepAngle">회전량 (+ : CCW, - : CW)</param>
        /// <returns></returns>
        public bool ListArc(float cx, float cy, float sweepAngle) => this.ListArc(new Vector2(cx, cy), sweepAngle);

        /// <summary>리스트 명령 - 마크 (Ellipse : 타원)</summary>
        /// <param name="vCenter">중심</param>
        /// <param name="majorHalf">A</param>
        /// <param name="minorHalf">B</param>
        /// <param name="startAngle">시작 각도</param>
        /// <param name="sweepAngle">각도 회전량 (+ : CCW, - : CW)</param>
        /// <param name="rotateAngle">타원 자체 회전량 (+ : CCW, - : CW)</param>
        /// <returns></returns>
        public bool ListEllipse(
          Vector2 vCenter,
          float majorHalf,
          float minorHalf,
          float startAngle,
          float sweepAngle,
          float rotateAngle = 0.0f)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            bool flag1 = true & this.ListJump(Vector2.Transform(Vector2.Transform(new Vector2(majorHalf * (float)Math.Cos((double)startAngle * (Math.PI / 180.0)), minorHalf * (float)Math.Sin((double)startAngle * (Math.PI / 180.0))), Matrix3x2.CreateRotation((float)Math.PI / 180f * rotateAngle)) + vCenter, this.MatrixStack.ToResult), 1f);
            for (float num = 0.0f; (double)num < (double)sweepAngle; ++num)
            {
                Vector2 vPosition = Vector2.Transform(Vector2.Transform(new Vector2(majorHalf * (float)Math.Cos(((double)startAngle + (double)num) * (double)Math.Sign(sweepAngle) * (Math.PI / 180.0)), minorHalf * (float)Math.Sin(((double)startAngle + (double)num) * (double)Math.Sign(sweepAngle) * (Math.PI / 180.0))), Matrix3x2.CreateRotation((float)Math.PI / 180f * rotateAngle)) + vCenter, this.MatrixStack.ToResult);
                flag1 &= this.ListMark(vPosition, 1f);
            }
            Vector2 vPosition1 = Vector2.Transform(Vector2.Transform(new Vector2(majorHalf * (float)Math.Cos(((double)startAngle + (double)sweepAngle) * (Math.PI / 180.0)), minorHalf * (float)Math.Sin(((double)startAngle + (double)sweepAngle) * (Math.PI / 180.0))), Matrix3x2.CreateRotation((float)Math.PI / 180f * rotateAngle)) + vCenter, this.MatrixStack.ToResult);
            bool flag2 = flag1 & this.ListMark(vPosition1, 1f);
            this.stream?.WriteLine(string.Format("ELLIPSE_BY_CENTER. MAJOR = {0:F3}, MINOR = {1:F3}, START_ANGLE = {2:F3}, SWEEP_ANGLE = {3:F3}, ANGLE = {4:F3}", (object)majorHalf, (object)minorHalf, (object)startAngle, (object)sweepAngle, (object)rotateAngle));
            return flag2;
        }

        /// <summary>Conic 베지어 곡선</summary>
        /// <param name="vStart">시작 위치</param>
        /// <param name="vControl">제어점 위치</param>
        /// <param name="vEnd">끝 위치</param>
        /// <param name="drawLength">직선 보간 거리 (mm)</param>
        /// <returns></returns>
        public bool ListConicBezier(Vector2 vStart, Vector2 vControl, Vector2 vEnd, float drawLength = 0.0f)
        {
            bool flag1 = true;
            float num1 = Vector2.Distance(this.vLogical, vControl);
            float num2 = Vector2.Distance(vControl, vEnd);
            float t = 0.0f;
            float num3 = 1f / (0.0 != (double)drawLength ? (num1 + num2) / drawLength : 4f);
            bool flag2 = flag1 & this.ListJump(vStart, 1f);
            do
            {
                float x = (float)((double)vStart.X * (double)MathHelper.CONIC_B1(t) + (double)vControl.X * (double)MathHelper.CONIC_B2(t) + (double)vEnd.X * (double)MathHelper.CONIC_B3(t));
                float y = (float)((double)vStart.Y * (double)MathHelper.CONIC_B1(t) + (double)vControl.Y * (double)MathHelper.CONIC_B2(t) + (double)vEnd.Y * (double)MathHelper.CONIC_B3(t));
                flag2 &= this.ListMark(new Vector2(x, y), 1f);
                t += num3;
            }
            while (flag2 && (double)t < 1.0);
            if (flag2)
                flag2 &= this.ListMark(vEnd, 1f);
            return flag2;
        }

        /// <summary>Cubic 베지어 곡선</summary>
        /// <param name="vStart">시작 위치</param>
        /// <param name="vControl1">제어점1 위치</param>
        /// <param name="vControl2">제어점2 위치</param>
        /// <param name="vEnd">끝 위치</param>
        /// <param name="drawLength">직선 보간 거리 (mm)</param>
        /// <returns></returns>
        public bool ListCubicBezier(
          Vector2 vStart,
          Vector2 vControl1,
          Vector2 vControl2,
          Vector2 vEnd,
          float drawLength = 0.0f)
        {
            bool flag1 = true;
            float num1 = Vector2.Distance(this.vLogical, vControl1);
            float num2 = Vector2.Distance(vControl1, vControl2);
            float num3 = Vector2.Distance(vControl2, vEnd);
            float t = 0.0f;
            float num4 = 1f / (0.0 != (double)drawLength ? (num1 + num2 + num3) / drawLength : 5f);
            bool flag2 = flag1 & this.ListJump(vStart, 1f);
            do
            {
                float x = (float)((double)vStart.X * (double)MathHelper.CUBIC_B1(t) + (double)vControl1.X * (double)MathHelper.CUBIC_B2(t) + (double)vControl2.X * (double)MathHelper.CUBIC_B3(t) + (double)vEnd.X * (double)MathHelper.CUBIC_B4(t));
                float y = (float)((double)vStart.Y * (double)MathHelper.CUBIC_B1(t) + (double)vControl1.Y * (double)MathHelper.CUBIC_B2(t) + (double)vControl2.X * (double)MathHelper.CUBIC_B3(t) + (double)vEnd.Y * (double)MathHelper.CUBIC_B4(t));
                flag2 &= this.ListMark(new Vector2(x, y), 1f);
                t += num4;
            }
            while (flag2 && (double)t < 1.0);
            if (flag2)
                flag2 &= this.ListMark(vEnd, 1f);
            return flag2;
        }

        /// <summary>리스트 명령 - 확장 포트에 데이타 쓰기</summary>
        /// <param name="ch">확장 커넥터 종류</param>
        /// <param name="value">값(16비트, 8비트, 2비트(int), 아나로그(float 10V)</param>
        /// <param name="compensator">compensator 보정용 객체</param>
        /// <returns></returns>
        public bool ListWriteData<T>(ExtensionChannel ch, T value, ICompensator<T> compensator = null)
        {
            if (!this.IsListReady(2U))
                return false;
            RTC4Wrap.n_list_nop((ushort)(this.Index + 1U));
            if (compensator != null)
            {
                T output;
                if (!compensator.Interpolate(value, out output))
                    return false;
                value = output;
            }
            switch (ch)
            {
                case ExtensionChannel.ExtDO8:
                    ushort num1 = (ushort)Convert.ChangeType((object)value, typeof(uint));
                    RTC4Wrap.n_write_8bit_port_list((ushort)(this.Index + 1U), num1);
                    break;
                case ExtensionChannel.ExtDO16:
                    ushort num2 = (ushort)Convert.ChangeType((object)value, typeof(uint));
                    RTC4Wrap.n_write_io_port_list((ushort)(this.Index + 1U), num2);
                    break;
                case ExtensionChannel.ExtAO1:
                    double num3 = (double)Convert.ChangeType((object)value, typeof(double));
                    double num4 = (Math.Pow(2.0, 10.0) - 1.0) * num3 / 10.0;
                    RTC4Wrap.n_write_da_x_list((ushort)(this.Index + 1U), (ushort)1, (ushort)num4);
                    break;
                case ExtensionChannel.ExtAO2:
                    double num5 = (double)Convert.ChangeType((object)value, typeof(double));
                    double num6 = (Math.Pow(2.0, 10.0) - 1.0) * num5 / 10.0;
                    RTC4Wrap.n_write_da_x_list((ushort)(this.Index + 1U), (ushort)2, (ushort)num6);
                    break;
                default:
                    return false;
            }
            this.stream?.WriteLine("DATA : " + ch.ToString() + " : " + value.ToString());
            return true;
        }

        /// <summary>확장1 포트의 16비트 디지털 출력의 특정 비트값을 변경</summary>
        /// <param name="bitPosition">0~15</param>
        /// <param name="onOff">출력</param>
        /// <returns></returns>
        public bool ListWriteExtDO16(ushort bitPosition, bool onOff) => false;

        /// <summary>리스트 명령 끝 - 버퍼 닫기</summary>
        /// <returns></returns>
        /// s
        public bool ListEnd()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(2U))
                return false;
            RTC4Wrap.n_set_end_of_list((ushort)(this.Index + 1U));
            this.stream?.WriteLine("; LIST ENDED : " + DateTime.Now.ToString());
            this.stream?.WriteLine(Environment.NewLine);
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: list has ended. counts= {1}", (object)this.Index, (object)this.listTotalCount), Array.Empty<object>());
            return true;
        }

        /// <summary>리스트 명령 실행</summary>
        /// <param name="busyWait">모든 리스트 명령의 실행이 완료될때까지 대기</param>
        /// <returns></returns>
        public bool ListExecute(bool busyWait = true)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            switch (this.listType)
            {
                case ListType.Single:
                    RTC4Wrap.n_execute_list((ushort)(this.Index + 1U), this.listIndex);
                    break;
                case ListType.Auto:
                    ushort busy;
                    ushort position;
                    RTC4Wrap.n_get_status((ushort)(this.Index + 1U), out busy, out position);
                    if (busy != (ushort)0)
                    {
                        RTC4Wrap.n_auto_change((ushort)(this.Index + 1U));
                        break;
                    }
                    RTC4Wrap.n_execute_list((ushort)(this.Index + 1U), this.listIndex);
                    break;
            }
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: list executing", (object)this.Index), Array.Empty<object>());
            if (busyWait)
                this.CtlBusyWait();
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.stream?.Flush();
            this.stream?.Dispose();
            this.stream = (StreamWriter)null;
            return true;
        }

        protected bool IsDuplicated(Vector2 vPosition)
        {
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            return !this.Is3D && MathHelper.IsEqual(this.vPhysical.X, vector2.X) && MathHelper.IsEqual(this.vPhysical.Y, vector2.Y);
        }

        protected bool IsListReady(uint count)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            switch (this.listType)
            {
                case ListType.Single:
                    this.listTotalCount += count;
                    return this.listTotalCount < 8000U;
                case ListType.Auto:
                    if (this.listTempCount + count >= this.RTC4_LIST_BUFFER_MAX)
                    {
                        ushort busy;
                        ushort position;
                        RTC4Wrap.n_get_status((ushort)(this.Index + 1U), out busy, out position);
                        if (busy != (ushort)0)
                        {
                            RTC4Wrap.n_set_end_of_list((ushort)(this.Index + 1U));
                            RTC4Wrap.n_execute_list((ushort)(this.Index + 1U), this.listIndex);
                            this.listIndex ^= (ushort)3;
                            RTC4Wrap.n_set_start_list((ushort)(this.Index + 1U), this.listIndex);
                        }
                        else
                        {
                            RTC4Wrap.n_set_end_of_list((ushort)(this.Index + 1U));
                            if (this.CtlGetStatus(RtcStatus.Aborted))
                                return false;
                            RTC4Wrap.n_auto_change((ushort)(this.Index + 1U));
                            switch (this.listIndex)
                            {
                                case 1:
                                    uint num1;
                                    do
                                    {
                                        num1 = (uint)RTC4Wrap.n_read_status((ushort)(this.Index + 1U));
                                        Thread.Sleep(10);
                                    }
                                    while (Convert.ToBoolean(num1 & 32U));
                                    break;
                                case 2:
                                    uint num2;
                                    do
                                    {
                                        num2 = (uint)RTC4Wrap.n_read_status((ushort)(this.Index + 1U));
                                        Thread.Sleep(10);
                                    }
                                    while (Convert.ToBoolean(num2 & 16U));
                                    break;
                            }
                            if (this.CtlGetStatus(RtcStatus.Aborted))
                                return false;
                            this.listIndex ^= (ushort)3;
                            RTC4Wrap.n_set_start_list((ushort)(this.Index + 1U), this.listIndex);
                        }
                        this.listTempCount = 0U;
                    }
                    this.listTempCount += count;
                    this.listTotalCount += count;
                    break;
            }
            return true;
        }

        /// <summary>지정된 위치를 홈(Home) 위치로 설정</summary>
        /// <param name="vPosition">X,Y (mm)</param>
        /// <returns></returns>
        public bool CtlHomePosition(Vector2 vPosition)
        {
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            int num1 = (int)((double)vector2.X * (double)this.KFactor);
            int num2 = (int)((double)vector2.Y * (double)this.KFactor);
            RTC4Wrap.n_home_position((ushort)(this.Index + 1U), (short)num1, (short)num2);
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: home position: {1:F3}, {2:F3}", (object)this.Index, (object)vector2.X, (object)vector2.Y), Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 위치를 홈(Home) 위치로 설정</summary>
        /// <param name="x">x mm</param>
        /// <param name="y">y mm</param>
        /// <returns></returns>
        public bool CtlHomePosition(float x, float y) => this.CtlHomePosition(new Vector2(x, y));

        /// <summary>FPK(First Pulse Killer) 시간값 설정</summary>
        /// <param name="usec">usec</param>
        /// <returns></returns>
        public bool CtlFirstPulseKiller(float usec)
        {
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            RTC4Wrap.n_set_firstpulse_killer((ushort)(this.Index + 1U), (ushort)((double)usec * 8.0));
            this.Fpk = usec;
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: fpk= {1:F1} usec", (object)this.Index, (object)usec), Array.Empty<object>());
            return true;
        }

        /// <summary>레이저 출력 신호 레벨 설정 (RTC4는 미지원. 보드상의 접점을 납땜하시오)</summary>
        /// <param name="laserControlSignal">RTC 모델에 맞는 ILaserControlSignal 구현된 인스턴스</param>
        /// <returns></returns>
        public bool CtlLaserSignalLevel(ILaserControlSignal laserControlSignal) => false;

        /// <summary>외부 트리거 사용시 설정</summary>
        /// <param name="mode">RTC 15핀 입력으로 /START, /STOP 등의 트리거 사용여부 설정</param>
        /// <param name="maxStartCounts">/START 트리거 최대 허용 개수 설정</param>
        /// <returns></returns>
        public bool CtlExternalControl(IRtcExternalControlMode mode, uint maxStartCounts = 0)
        {
            RTC4Wrap.n_set_control_mode((ushort)(this.Index + 1U), (ushort)mode.ToUInt());
            RTC4Wrap.n_set_max_counts((ushort)(this.Index + 1U), (int)maxStartCounts);
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: external control= {1}, max= {2}", (object)this.Index, (object)mode.ToUInt(), (object)maxStartCounts), Array.Empty<object>());
            return true;
        }

        /// <summary>외부 /START 실행된 회수 조회</summary>
        /// <param name="counts">회수값</param>
        /// <returns></returns>
        public bool CtlExternalStartCounts(out uint counts)
        {
            counts = (uint)RTC4Wrap.n_get_counts((ushort)(this.Index + 1U));
            return true;
        }

        /// <summary>FPK(First Pulse Killer) 시간값 설정</summary>
        /// <param name="usec">usec</param>
        /// <returns></returns>
        public bool ListFirstPulseKiller(float usec)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            RTC4Wrap.n_set_firstpulse_killer_list((ushort)(this.Index + 1U), (ushort)((double)usec * 8.0));
            this.Fpk = usec;
            this.stream?.WriteLine(string.Format("FIRST PULSE KILLER = {0:F3} usec", (object)usec));
            return true;
        }

        /// <summary>RTC4 카드는 미지원</summary>
        /// <param name="laserOnShift">usec</param>
        /// <param name="timeLag">usec</param>
        /// <param name="angularLimit">활성화될 각도 설정 (예: 90도)</param>
        /// <returns></returns>
        public bool ListSkyWriting(float laserOnShift, float timeLag, float angularLimit) => !this.CtlGetStatus(RtcStatus.Aborted);

        /// <summary>리스트 명령 - 레스터 처리 (Pixel Raster Operation)</summary>
        /// <param name="usec">매 픽셀의 주기 시간 (usec) : 가공 속도를 결정</param>
        /// <param name="vDelta">픽셀간 간격 (dx, dy) (mm)</param>
        /// <param name="pixelCount">한줄을 구성하는 픽셀의 개수</param>
        /// <param name="ext">아나로그 1 or 2 반드시 선택</param>
        /// <returns></returns>
        public bool ListPixelLine(float usec, Vector2 vDelta, uint pixelCount, ExtensionChannel ext = ExtensionChannel.ExtAO2)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(2U + pixelCount))
                return false;
            Matrix3x2 toResult = this.MatrixStack.ToResult;
            toResult.M31 = 0.0f;
            toResult.M32 = 0.0f;
            Vector2 vector2 = Vector2.Transform(vDelta, toResult);
            int num1 = (int)((double)vector2.X * (double)this.KFactor);
            int num2 = (int)((double)vector2.Y * (double)this.KFactor);
            if (ext == ExtensionChannel.ExtAO2)
            {
                RTC4Wrap.n_set_pixel_line((ushort)(this.Index + 1U), (ushort)1, (ushort)((double)usec / 10.0), (double)num1, (double)num2);
                this.pixelExtensionChannel = ext;
                return true;
            }
            Logger.Log(Logger.Type.Warn, string.Format("rtc4 [{0}]: {1} is not supported in ListPixelLine", (object)this.Index, (object)ext), Array.Empty<object>());
            return false;
        }

        /// <summary>
        /// 리스트 명령 - 개별 픽셀 명령
        /// 반드시 ListPixelLine 명령이 호출된후에 픽셀 개수만큼의 ListPixel 함수가 호출되어야 함
        /// </summary>
        /// <param name="usec">현재 픽셀의 출력 주기(lower than usec in ListPixelLine ) </param>
        /// <param name="weight">ExtensionChannel 출력의 가중치 값(0~1), 아나로그 10V</param>
        /// <param name="compensator">아나로그 출력값 보정기 사용시 지정</param>
        /// <returns></returns>
        public bool ListPixel(float usec, float weight = 0.0f, ICompensator<float> compensator = null)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if (compensator != null)
            {
                float output;
                if (!compensator.Interpolate(weight, out output))
                    return false;
                weight = output;
            }
            switch (this.pixelExtensionChannel)
            {
                case ExtensionChannel.ExtAO1:
                case ExtensionChannel.ExtAO2:
                    uint num = (uint)((Math.Pow(2.0, 10.0) - 1.0) * (double)weight);
                    RTC4Wrap.n_set_pixel((ushort)(this.Index + 1U), (ushort)((double)usec * 8.0), (ushort)num, (ushort)0);
                    return true;
                default:
                    Logger.Log(Logger.Type.Warn, string.Format("rtc4 [{0}]: {1} is not supported in ListPixelLine", (object)this.Index, (object)this.pixelExtensionChannel), Array.Empty<object>());
                    return false;
            }
        }

        /// <summary>리스트 명령 - 위 ListPixelLine + ListPixel * n 을 통합한 편이용 함수</summary>
        /// <param name="vStart">가공 시작점</param>
        /// <param name="vEnd">가공 끝점</param>
        /// <param name="periodUsec">픽셀 주기 (usec)</param>
        /// <param name="usecValues">매 픽셀 가공 시간 배열</param>
        /// <param name="ext">아나로그 확장 출력 1,2 지정</param>
        /// <param name="analogValues">아나로그 값(0~10) 배열</param>
        /// <param name="usecCompensator">픽셀 출력시간값 보정기 사용시</param>
        /// <param name="analogCompensator">아나로그 출력 보정기 사용시</param>
        /// <returns></returns>
        public bool ListPixels(
          Vector2 vStart,
          Vector2 vEnd,
          float periodUsec,
          float[] usecValues,
          ExtensionChannel ext = ExtensionChannel.ExtAO1,
          float[] analogValues = null,
          ICompensator<float> usecCompensator = null,
          ICompensator<float> analogCompensator = null)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            int length = usecValues.Length;
            bool flag = true & this.ListJump(vStart, 1f);
            Vector2 position;
            position.X = (vEnd.X - vStart.X) / (float)length;
            position.Y = (vEnd.Y - vStart.Y) / (float)length;
            if (!this.IsListReady((uint)(2 + length)))
                return false;
            Matrix3x2 toResult = this.MatrixStack.ToResult;
            toResult.M31 = 0.0f;
            toResult.M32 = 0.0f;
            Vector2 vector2 = Vector2.Transform(position, toResult);
            int num1 = (int)((double)vector2.X * (double)this.KFactor);
            int num2 = (int)((double)vector2.Y * (double)this.KFactor);
            if (ext != ExtensionChannel.ExtAO1 && ext == ExtensionChannel.ExtAO2)
                RTC4Wrap.n_set_pixel_line((ushort)(this.Index + 1U), (ushort)1, (ushort)((double)periodUsec / 10.0), (double)num1, (double)num2);
            else
                Logger.Log(Logger.Type.Warn, string.Format("rtc4 [{0}]: {1} is not supported in ListPixelLine", (object)this.Index, (object)ext), Array.Empty<object>());
            for (int index = 0; index < usecValues.Length; ++index)
            {
                if (this.CtlGetStatus(RtcStatus.Aborted))
                    return false;
                float input1 = usecValues[index];
                if (usecCompensator != null)
                {
                    float output;
                    if (!usecCompensator.Interpolate(input1, out output))
                        return false;
                    input1 = output;
                }
                float input2 = 0.0f;
                if (analogValues != null && analogValues.Length == length)
                {
                    input2 = analogValues[index];
                    if (analogCompensator != null)
                    {
                        float output;
                        if (!analogCompensator.Interpolate(input2, out output))
                            return false;
                        input2 = output;
                    }
                }
                uint num3 = (uint)((Math.Pow(2.0, 10.0) - 1.0) * (double)input2 / 10.0);
                RTC4Wrap.n_set_pixel((ushort)(this.Index + 1U), (ushort)((double)input1 * 8.0), (ushort)num3, (ushort)0);
            }
            return flag;
        }

        /// <summary>리스트 명령 - 와블 (Wobbel Operation)</summary>
        /// <param name="amplitudeX">size of W (parallel movement) (mm)</param>
        /// <param name="amplitudeY">size of Y (perpendicular movement) (mm)</param>
        /// <param name="frequencyHz">초당 반복회수 (Hz)</param>
        /// <returns></returns>
        public bool ListWobbel(float amplitudeX, float amplitudeY, float frequencyHz)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            uint val2 = (uint)((double)amplitudeX * (double)this.KFactor);
            uint val1 = (uint)((double)amplitudeY * (double)this.KFactor);
            RTC4Wrap.n_set_wobbel((ushort)(this.Index + 1U), (ushort)Math.Max(val1, val2), (double)this.frequency);
            return true;
        }

        /// <summary>RTC4 카드는 미지원</summary>
        /// <param name="offsetX">X 초기화 위치 (mm)</param>
        /// <param name="offsetY">Y 초기화 위치 (mm)</param>
        /// <returns></returns>
        public bool CtlEncoderReset(float offsetX = 0.0f, float offsetY = 0.0f)
        {
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: motf encoder reset is not supported !", (object)this.Index), Array.Empty<object>());
            return false;
        }

        /// <summary>외부 엔코더 입력 대신 내부 가상 엔코더를 활성화 및 가상 입력 엔코더 속도 지정</summary>
        /// <param name="encXSimulatedSpeed">RTC 내부 가상 엔코더X 속도 (mm/s)</param>
        /// <param name="encYSimulatedSpeed">RTC 내부 가상 엔코더Y 속도 (mm/s)</param>
        /// <returns></returns>
        public bool CtlEncoderSpeed(float encXSimulatedSpeed, float encYSimulatedSpeed)
        {
            if (0.0 == (double)encXSimulatedSpeed && 0.0 == (double)encYSimulatedSpeed)
                RTC4Wrap.n_simulate_encoder((ushort)(this.Index + 1U), (ushort)0);
            else if (0.0 != (double)encXSimulatedSpeed)
                RTC4Wrap.n_simulate_encoder((ushort)(this.Index + 1U), (ushort)1);
            else if (0.0 != (double)encYSimulatedSpeed)
                RTC4Wrap.n_simulate_encoder((ushort)(this.Index + 1U), (ushort)2);
            this.EncXSimulatedSpeed = encXSimulatedSpeed;
            this.EncYSimulatedSpeed = encYSimulatedSpeed;
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: motf encoder simulated with {1:F3}, {2:F3} mm/s", (object)this.Index, (object)encXSimulatedSpeed, (object)encYSimulatedSpeed), Array.Empty<object>());
            return true;
        }

        /// <summary>현재 엔코더 값 얻기</summary>
        /// <param name="encX">X 엔코더 값 (counts)</param>
        /// <param name="encY">Y 엔코더 값 (counts)</param>
        /// <param name="encXmm">X 엔코더의 위치 (mm)</param>
        /// <param name="encYmm">Y 엔코더의 위치 (mm)</param>
        /// <returns></returns>
        public bool CtlGetEncoder(out int encX, out int encY, out float encXmm, out float encYmm)
        {
            short zx = 0;
            short zy = 0;
            RTC4Wrap.n_get_encoder((ushort)(this.Index + 1U), out zx, out zy);
            encX = (int)zx;
            encY = (int)zy;
            encXmm = encYmm = 0.0f;
            if (this.EncXCountsPerMm != 0)
                encXmm = (float)encX / (float)this.EncXCountsPerMm;
            if (this.EncYCountsPerMm != 0)
                encYmm = (float)encY / (float)this.EncYCountsPerMm;
            return true;
        }

        /// <summary>RTC4 카드는 미지원</summary>
        /// <param name="xUsec">X 축 보상 시간(usec)</param>
        /// <param name="yUsec">Y축 보상 시간(usec)</param>
        /// <returns></returns>
        public bool CtlTrackingError(uint xUsec, uint yUsec)
        {
            Logger.Log(Logger.Type.Error, string.Format("rtc4 [{0}]: tracking is not supported !", (object)this.Index), Array.Empty<object>());
            return true;
        }

        /// <summary>RTC4 카드는 미지원</summary>
        /// <param name="fileName">보정 파일 이름 (경로포함), null 지정시 보정 리셋됨</param>
        /// <param name="tableNo">테이블 번호</param>
        /// <returns></returns>
        public bool CtlMotfCompensateTable(string fileName, uint tableNo = 0)
        {
            Logger.Log(Logger.Type.Error, string.Format("rtc4 [{0}]: compensate table is not supported !", (object)this.Index), Array.Empty<object>());
            return false;
        }

        /// <summary>외부 트리거 시작 (External Start) 사용시 지연 설정</summary>
        /// <param name="enc">x/Y 엔코더 종류</param>
        /// <param name="distance">엔코더 지연 거리 (mm)</param>
        /// <returns></returns>
        public bool CtlExternalControlDelay(RtcEncoder enc, float distance)
        {
            switch (enc)
            {
                case RtcEncoder.EncX:
                    RTC4Wrap.n_set_ext_start_delay((ushort)(this.Index + 1U), (short)((double)this.EncXCountsPerMm * (double)distance), (short)0);
                    break;
                case RtcEncoder.EncY:
                    RTC4Wrap.n_set_ext_start_delay((ushort)(this.Index + 1U), (short)((double)this.EncYCountsPerMm * (double)distance), (short)1);
                    break;
            }
            Logger.Log(Logger.Type.Info, string.Format("rtc4 [{0}]: motf external control delay distance= {1}, {2:F3} mm", (object)this.Index, (object)enc, (object)distance), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 리스트 명령 - MOTF 리스트 명령 시작
        /// 엔코더 값 초기화시에는 CtlEncoderReset에서 설정한 오프셋 값으로 초기화되며,
        /// 초기화를 하지 않더라도 ListBegin 시에는 외부 트리거 (/START)를 사용가능하도록 설정하기 때문에
        /// 해당 트리거 신호가 활성화(Closed)되면 엔코더가 자동으로 리셋(초기화) 되도록 설정됨
        /// </summary>
        /// <param name="encoderReset">엔코더 X,Y 초기화 여부 (RTC4는 무조건 초기화됨) (</param>
        /// <returns></returns>
        public bool ListMOTFBegin(bool encoderReset = false)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            double kx = 0.0;
            double ky = 0.0;
            if ((double)this.EncXSimulatedSpeed == 0.0 && (double)this.EncYSimulatedSpeed == 0.0)
            {
                if (this.EncXCountsPerMm != 0)
                    kx = (double)this.KFactor / (double)this.EncXCountsPerMm;
                if (this.EncYCountsPerMm != 0)
                    ky = (double)this.KFactor / (double)this.EncYCountsPerMm;
            }
            else if ((double)this.EncXSimulatedSpeed == 0.0)
            {
                if (this.EncXCountsPerMm == 0)
                {
                    Logger.Log(Logger.Type.Error, string.Format("rtc4 [{0}]: motf invalid encoder0 counts/mm value)", (object)this.Index), Array.Empty<object>());
                    return false;
                }
                kx = (double)this.KFactor / (double)this.EncXCountsPerMm;
                ky = (double)this.KFactor * (double)this.EncYSimulatedSpeed / 1000000.0;
            }
            else if ((double)this.EncYSimulatedSpeed == 0.0)
            {
                if (this.EncYCountsPerMm == 0)
                {
                    Logger.Log(Logger.Type.Error, string.Format("rtc4 [{0}]: motf invalid encoder1 counts/mm value)", (object)this.Index), Array.Empty<object>());
                    return false;
                }
                kx = (double)this.KFactor * (double)this.EncXSimulatedSpeed / 1000000.0;
                ky = (double)this.KFactor / (double)this.EncYCountsPerMm;
            }
            else
            {
                if (0.0 == (double)this.EncXSimulatedSpeed || 0.0 == (double)this.EncYSimulatedSpeed)
                {
                    Logger.Log(Logger.Type.Error, string.Format("rtc4 [{0}]: motf invalid encoder0,1 counts/mm value)", (object)this.Index), Array.Empty<object>());
                    return false;
                }
                kx = (double)this.KFactor * (double)this.EncXSimulatedSpeed / 1000000.0;
                ky = (double)this.KFactor * (double)this.EncYSimulatedSpeed / 1000000.0;
            }
            if (0.0 == kx)
                kx = ky;
            if (0.0 == ky)
                ky = kx;
            if (!this.IsListReady(1U))
                return false;
            RTC4Wrap.n_set_fly_x((ushort)(this.Index + 1U), kx);
            RTC4Wrap.n_set_fly_y((ushort)(this.Index + 1U), ky);
            return true;
        }

        /// <summary>리스트 명령 - 외부 트리거 시작 (External Start) 사용시 지연 설정</summary>
        /// <param name="enc">x/Y 엔코더 종류</param>
        /// <param name="distance">엔코더 지연 거리</param>
        /// <returns></returns>
        public bool ListExternalControlDelay(RtcEncoder enc, float distance)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            switch (enc)
            {
                case RtcEncoder.EncX:
                    RTC4Wrap.n_set_ext_start_delay_list((ushort)(this.Index + 1U), (short)((double)this.EncXCountsPerMm * (double)distance), (short)0);
                    break;
                case RtcEncoder.EncY:
                    RTC4Wrap.n_set_ext_start_delay_list((ushort)(this.Index + 1U), (short)((double)this.EncYCountsPerMm * (double)distance), (short)1);
                    break;
            }
            return true;
        }

        /// <summary>
        /// RTC4는 미지원
        /// (단축 동기화 용)
        /// </summary>
        /// <param name="enc">엔코더 축 지정</param>
        /// <param name="position">위치값 (mm)</param>
        /// <param name="cond">대기 조건</param>
        /// <returns></returns>
        public bool ListMOTFWait(RtcEncoder enc, float position, EncoderWaitCondition cond) => true;

        /// <summary>
        /// RTC4는 미지원
        /// /// </summary>
        /// <param name="positionX">X 축 위치 (mm)</param>
        /// <param name="rangeX">조건 범위 (mm)</param>
        /// <param name="positionY">Y 축 위치 (mm)</param>
        /// <param name="rangeY">조건 범위 (mm)</param>
        /// <returns></returns>
        public bool ListMOTFWaits(float positionX, float rangeX, float positionY, float rangeY) => true;

        /// <summary>
        /// 리스트 명령 - MOTF 로 동작하는 리스트 명령 끝
        /// MOTF 종료시 스캐너를 지정된 위치로 점프 가능
        /// </summary>
        /// <param name="vPosition">점프 위치 (x,y) (mm)</param>
        /// <returns></returns>
        public bool ListMOTFEnd(Vector2 vPosition)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            int num1 = (int)((double)vector2.X * (double)this.KFactor);
            int num2 = (int)((double)vector2.Y * (double)this.KFactor);
            if (!this.IsListReady(1U))
                return false;
            RTC4Wrap.n_fly_return((ushort)(this.Index + 1U), (short)num1, (short)num2);
            this.vPhysical = vector2;
            this.vLogical = vPosition;
            return true;
        }
    }
}
