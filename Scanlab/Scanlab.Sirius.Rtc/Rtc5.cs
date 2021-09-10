// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Rtc5
// Assembly: spirallab.sirius.rtc, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 330B13B0-CD9B-4679-A17E-EBB26CA3FE4F
// Assembly location: C:\Users\sean0\AppData\Local\Temp\Hevabaq\70527ef0f3\sirius-master\bin\spirallab.sirius.rtc.dll

using RTC5Import;
using System;
using System.IO;
using System.Numerics;
using System.Threading;

namespace Scanlab.Sirius
{
    /// <summary>RTC5 객체</summary>
    public class Rtc5 :
      IRtc,
      IDisposable,
      IRtcExtension,
      IRtcDualHead,
      IRtc3D,
      IRtcMOTF,
      IRtcCharacterSet,
      IRtcAutoLaserControl
    {
        /// <summary>카드 개수</summary>
        public static uint Count;
        private string[] ctbFileName = new string[4];
        /// <summary>물리적인 마지막 위치값</summary>
        protected Vector3 vPhysical3D;
        /// <summary>논리적인 마지막 위치값</summary>
        protected Vector3 vLogical3D;
        /// <summary>엔코더 X 의 가상 속도 mm/s</summary>
        private float EncXSimulatedSpeed;
        /// <summary>엔코더 Y 의 가상 속도 mm/s</summary>
        private float EncYSimulatedSpeed;
        protected bool isAborted;
        protected uint listIndex;
        protected uint listTempCount;
        protected uint listTotalCount;
        protected bool extCtrlMode;
        protected readonly uint RTC5_LIST_BUFFER_MAX = 10000;
        protected readonly uint RTC5_LIST3_BUFFER_SIZE = (uint)Math.Pow(2.0, 17.0);
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
        protected bool isListAlcByVectorBegin;
        private AutoLaserControlSignal byVectorAlcControlSignal;

        /// <summary>RTC 제어기 식별 번호 (1,2,3,...)</summary>
        public uint Index { get; set; }

        /// <summary>이름</summary>
        public string Name { get; set; }

        /// <summary>행렬 스택</summary>
        public IMatrixStack MatrixStack { get; set; }

        /// <summary>bits/mm 값 (2^20)</summary>
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

        /// <summary>스캐너 보정 파일 목록 (최대 4개 로드 가능)</summary>
        public string[] CorrectionFiles => this.ctbFileName;

        /// <summary>첫번째 스캐너 헤드 보정 테이블 번호 (1~4)</summary>
        public CorrectionTableIndex PrimaryHeadTable { get; protected set; }

        /// <summary>두번째 스캐너 헤드 보정 테이블 번호 (1~4)</summary>
        public CorrectionTableIndex SecondaryHeadTable { get; protected set; }

        /// <summary>첫번째 스캐너 헤드 X,Y 오프셋 (mm)</summary>
        public Vector2 PrimaryHeadOffset { get; protected set; }

        /// <summary>첫번째 스캔 헤드 회전 (각도)</summary>
        public float PrimaryHeadAngle { get; protected set; }

        /// <summary>두번째 스캐너 헤드 X,Y 오프셋 (mm)</summary>
        public Vector2 SecondaryHeadOffset { get; protected set; }

        /// <summary>두번째 스캔 헤드 회전 (각도)</summary>
        public float SecondaryHeadAngle { get; protected set; }

        /// <summary>Z 이동량 오프셋 (mm)</summary>
        public float ZOffset { get; protected set; }

        /// <summary>Z 디포커스 값 (mm)</summary>
        public float ZDefocus { get; protected set; }

        /// <summary>Z 축 bits/mm 값</summary>
        public float KZFactor { get; protected set; }

        /// <summary>
        /// ALC(Automatic Laser Control 중 위치 의존적 방법으로 Scale 값이 외부 파일에서 제공됨
        /// Null 지정후 CtlAutoLaserControl 호출하면 비활성화됨
        /// 포맷
        /// [PositionCtrlTable No]
        /// PositionNo = Value
        /// ScaleNo = Value
        /// ...
        ///  N 1-50
        /// Position : 스캐너 중심으로 부터 떨어진 거리 (반지름) 의 퍼센트값: 100 % =  2^19 bits (RTC5 경우)
        /// Scale : 0- 4
        /// </summary>
        public string AutoLaserControlByPositionFileName { get; set; }

        /// <summary>
        /// ALC(Automatic Laser Control 중 위치 의존적 방법으로 어떤 테이블을 사용할지 지정
        /// </summary>
        public uint AutoLaserControlByPositionTableNo { get; set; }

        /// <summary>단위 mm 당 엔코더 X 개수</summary>
        public int EncXCountsPerMm { get; set; }

        /// <summary>단위 mm 당 엔코더 Y 개수</summary>
        public int EncYCountsPerMm { get; set; }

        /// <summary>시작 시리얼 번호값 (CtlSerialReset 함수에 의해 설정)</summary>
        public uint SerialStartNo { get; private set; }

        /// <summary>증가 시리얼 번호값 (CtlSerialReset 함수에 의해 설정)</summary>
        public uint SerialIncrementStep { get; private set; }

        /// <summary>현재 시리얼 번호값 (외부 /START 에 의해 증가된 값)</summary>
        public uint SerialCurrentNo
        {
            get
            {
                uint counts;
                this.CtlExternalStartCounts(out counts);
                return counts * this.SerialIncrementStep + this.SerialStartNo;
            }
        }

        /// <summary>생성자</summary>
        public Rtc5()
        {
            this.Index = 0U;
            this.outputFileName = string.Empty;
            this.isAborted = false;
            this.listIndex = 1U;
            this.listTempCount = 0U;
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
            ++Rtc5.Count;
        }

        /// <summary>생성자</summary>
        /// <param name="index">카드번호 (0,1,2,...)</param>
        /// <param name="outputFileName">리스트 명령 로그 출력 파일 이름</param>
        public Rtc5(uint index, string outputFileName = "")
          : this()
        {
            this.Index = index;
            this.outputFileName = outputFileName;
        }

        /// <summary>종결자</summary>
        ~Rtc5()
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
            {
                --Rtc5.Count;
                if (Rtc5.Count == 0U)
                    RTC5Wrap.free_rtc5_dll();
            }
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
                //this.rockey.InvalidLicense();
            this.KFactor = kFactor;
            if (!RTC5Wrap.Initialized)
            {
                uint num = RTC5Wrap.init_rtc5_dll();
                if (num != 0U)
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: fail to init_rtc5_dll = {1}", (object)this.Index, (object)num), Array.Empty<object>());
                RTC5Wrap.Initialized = num == 0U;
            }
            RTC5Wrap.n_stop_execution(this.Index + 1U);
            uint num1 = RTC5Wrap.n_load_program_file(this.Index + 1U, string.Empty);
            if (num1 != 0U)
                Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: fail to load_program_file = {1}", (object)this.Index, (object)num1), Array.Empty<object>());
            if (num1 != 0U)
            {
                switch ((int)num1 - 1)
                {
                    case 0:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: file error (file corrupt or incomplete)", (object)this.Index), Array.Empty<object>());
                        break;
                    case 1:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: memory error (DLL-internal, WINDOWS system memory)", (object)this.Index), Array.Empty<object>());
                        break;
                    case 2:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: File open error (empty string summitted or file not found)", (object)this.Index), Array.Empty<object>());
                        break;
                    case 3:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: DSP memory error", (object)this.Index), Array.Empty<object>());
                        break;
                    case 4:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: PCI download error (driver error)", (object)this.Index), Array.Empty<object>());
                        break;
                    case 7:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: System driver not found", (object)this.Index), Array.Empty<object>());
                        break;
                    case 9:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: Parameter error (incorrect No.)", (object)this.Index), Array.Empty<object>());
                        break;
                    case 10:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: Access error : board reserved for another application", (object)this.Index), Array.Empty<object>());
                        break;
                    case 11:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: Warning : 3D table or D3 selected but 3D option is not enabled.", (object)this.Index), Array.Empty<object>());
                        break;
                    case 12:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: Busy error: no download, board is BUSY or INTERNAL BUSY", (object)this.Index), Array.Empty<object>());
                        break;
                    case 13:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: PCI upload error (driver error, only applicable for download verification)", (object)this.Index), Array.Empty<object>());
                        break;
                    case 14:
                        Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: Verify error (only applicable for download verification)", (object)this.Index), Array.Empty<object>());
                        break;
                }
                return false;
            }
            uint Code = RTC5Wrap.n_get_last_error(this.Index + 1U);
            if (Code != 0U)
                RTC5Wrap.n_reset_error(this.Index + 1U, Code);
            int num2 = (int)RTC5Wrap.rtc5_count_cards();
            int num3 = (int)RTC5Wrap.get_dll_version();
            int num4 = (int)RTC5Wrap.get_hex_version();
            uint num5 = RTC5Wrap.get_rtc_version();
            uint num6 = RTC5Wrap.n_get_serial_number(this.Index + 1U);
            this.IsMOTF = Convert.ToBoolean(num5 & 256U);
            this.Is2ndHead = Convert.ToBoolean(num5 & 512U);
            this.Is3D = Convert.ToBoolean(num5 & 1024U);
            RTC5Wrap.n_set_laser_control(this.Index + 1U, 256U);
            if (!this.CtlLoadCorrectionFile(CorrectionTableIndex.Table1, ctbFileName))
                return false;
            if (!this.Is3D)
            {
                if (!this.CtlSelectCorrection(CorrectionTableIndex.Table1, CorrectionTableIndex.None))
                    return false;
            }
            else if (!this.CtlSelectCorrection(CorrectionTableIndex.Table1, CorrectionTableIndex.Table1))
                return false;
            this.KZFactor = kFactor / 16f;
            this.LaserMode = laserMode;
            this.Fpk = 0.0f;
            RTC5Wrap.n_set_laser_mode(this.Index + 1U, (uint)laserMode);
            RTC5Wrap.n_set_firstpulse_killer(this.Index + 1U, 0U);
            RTC5Wrap.n_set_standby(this.Index + 1U, 0U, 0U);
            RTC5Wrap.n_time_update(this.Index + 1U);
            RTC5Wrap.n_config_list(this.Index + 1U, this.RTC5_LIST_BUFFER_MAX * 2U, this.RTC5_LIST_BUFFER_MAX * 2U);
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: serial no = {1} initialized", (object)this.Index, (object)num6), Array.Empty<object>());
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
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            switch (RTC5Wrap.n_load_correction_file(this.Index + 1U, ctbFileName, (uint)(tableIndex + 1), this.Is3D ? 3U : 2U))
            {
                case 0:
                    this.ctbFileName[(int)tableIndex] = ctbFileName;
                    Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: correction file loaded : {1} at {2}", (object)this.Index, (object)ctbFileName, (object)tableIndex.ToString()), Array.Empty<object>());
                    return true;
                case 1:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: file error (file corrupt or incomplete)", (object)this.Index), Array.Empty<object>());
                    break;
                case 2:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: memory error (DLL-internal, WINDOWS system memory)", (object)this.Index), Array.Empty<object>());
                    break;
                case 3:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: File open error (empty string summitted or file not found)", (object)this.Index), Array.Empty<object>());
                    break;
                case 4:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: DSP memory error", (object)this.Index), Array.Empty<object>());
                    break;
                case 5:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: PCI download error (driver error)", (object)this.Index), Array.Empty<object>());
                    break;
                case 8:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: System driver not found", (object)this.Index), Array.Empty<object>());
                    break;
                case 10:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: Parameter error (incorrect No.)", (object)this.Index), Array.Empty<object>());
                    break;
                case 11:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: Access error : board reserved for another application", (object)this.Index), Array.Empty<object>());
                    break;
                case 12:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: Warning : 3D table or D3 selected but 3D option is not enabled.", (object)this.Index), Array.Empty<object>());
                    break;
                case 13:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: Busy error: no download, board is BUSY or INTERNAL BUSY", (object)this.Index), Array.Empty<object>());
                    break;
                case 14:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: PCI upload error (driver error, only applicable for download verification)", (object)this.Index), Array.Empty<object>());
                    break;
                case 15:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: Verify error (only applicable for download verification)", (object)this.Index), Array.Empty<object>());
                    break;
            }
            return false;
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
                RTC5Wrap.n_select_cor_table(this.Index + 1U, (uint)(primaryHeadTableIndex + 1), (uint)(secondaryHeadTableIndex + 1));
                this.PrimaryHeadTable = primaryHeadTableIndex;
                this.SecondaryHeadTable = secondaryHeadTableIndex;
            }
            else
            {
                RTC5Wrap.n_select_cor_table(this.Index + 1U, (uint)(primaryHeadTableIndex + 1), (uint)(secondaryHeadTableIndex + 1));
                this.PrimaryHeadTable = primaryHeadTableIndex;
                this.SecondaryHeadTable = secondaryHeadTableIndex;
            }
            // ISSUE: variable of a boxed type
            uint index = this.Index;
            CorrectionTableIndex correctionTableIndex = this.PrimaryHeadTable;
            string str1 = correctionTableIndex.ToString();
            correctionTableIndex = this.SecondaryHeadTable;
            string str2 = correctionTableIndex.ToString();
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: correction file selected : primary head= {1}. secondary head= {2}", (object)index, (object)str1, (object)str2), Array.Empty<object>());
            return true;
        }

        /// <summary>현재 설정된 주파수, 펄스폭 으로 레이저 변조 신호(LASER1,2,ON) 출력</summary>
        /// <returns></returns>
        public bool CtlLaserOn()
        {
            RTC5Wrap.n_laser_signal_on(this.Index + 1U);
            this.isManualOn = true;
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: laser is on by manually", (object)this.Index), Array.Empty<object>());
            return true;
        }

        /// <summary>레이저 변호 신호 (LASER1,2,ON) 중단</summary>
        /// <returns></returns>
        public bool CtlLaserOff()
        {
            RTC5Wrap.n_laser_signal_off(this.Index + 1U);
            this.isManualOn = false;
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: laser is off by manually", (object)this.Index), Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 위치로 스캐너 수동 이동</summary>
        /// <param name="vPosition">X,Y (mm)</param>
        /// <returns></returns>
        public bool CtlMove(Vector2 vPosition)
        {
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            int X = (int)((double)vector2.X * (double)this.KFactor);
            int Y = (int)((double)vector2.Y * (double)this.KFactor);
            RTC5Wrap.n_goto_xy(this.Index + 1U, X, Y);
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: scanner moved : {1:F3}, {2:F3}", (object)this.Index, (object)vector2.X, (object)vector2.Y), Array.Empty<object>());
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
            double num = 1.0 / (double)frequency * 1000000.0 / 2.0;
            RTC5Wrap.n_set_start_list(this.Index + 1U, 1U);
            RTC5Wrap.n_set_laser_timing(this.Index + 1U, (uint)(num * 64.0), (uint)((double)pulseWidth * 64.0), (uint)((double)pulseWidth * 64.0), 0U);
            RTC5Wrap.n_set_end_of_list(this.Index + 1U);
            RTC5Wrap.n_execute_list(this.Index + 1U, 1U);
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
            RTC5Wrap.n_set_start_list(this.Index + 1U, 1U);
            RTC5Wrap.n_set_scanner_delays(this.Index + 1U, (uint)((double)scannerJump / 10.0), (uint)((double)scannerMark / 10.0), (uint)((double)scannerPolygon / 10.0));
            RTC5Wrap.n_set_laser_delays(this.Index + 1U, (int)((double)laserOn * 2.0), (uint)((double)laserOff * 2.0));
            RTC5Wrap.n_set_end_of_list(this.Index + 1U);
            RTC5Wrap.n_execute_list(this.Index + 1U, 1U);
            this.CtlBusyWait();
            return true;
        }

        /// <summary>스캐너 점프/마크 속도 설정</summary>
        /// <param name="jump">점프(jump) 속도 (mm/s)</param>
        /// <param name="mark">마크(mark) 및 아크(arc) 속도 (mm/s)</param>
        /// <returns></returns>
        public bool CtlSpeed(float jump, float mark)
        {
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            double Speed1 = (double)jump / 1000.0 * (double)this.KFactor;
            double Speed2 = (double)mark / 1000.0 * (double)this.KFactor;
            if (Speed1 < 1.6)
                Speed1 = 1.6;
            if (Speed1 > 800000.0)
                Speed1 = 800000.0;
            if (Speed2 < 1.6)
                Speed2 = 1.6;
            if (Speed2 > 800000.0)
                Speed2 = 800000.0;
            RTC5Wrap.n_set_jump_speed_ctrl(this.Index + 1U, Speed1);
            RTC5Wrap.n_set_mark_speed_ctrl(this.Index + 1U, Speed2);
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
                    uint num2 = (uint)Convert.ChangeType((object)value, typeof(uint));
                    RTC5Wrap.n_write_8bit_port(this.Index + 1U, num2);
                    break;
                case ExtensionChannel.ExtDO16:
                    uint num3 = (uint)Convert.ChangeType((object)value, typeof(uint));
                    RTC5Wrap.n_write_io_port(this.Index + 1U, num3);
                    break;
                case ExtensionChannel.ExtAO1:
                    float num4 = (float)Convert.ChangeType((object)value, typeof(float));
                    uint num5 = (uint)((Math.Pow(2.0, 12.0) - 1.0) * (double)num4 / 10.0);
                    RTC5Wrap.n_write_da_x(this.Index + 1U, 1U, num5);
                    break;
                case ExtensionChannel.ExtAO2:
                    float num6 = (float)Convert.ChangeType((object)value, typeof(float));
                    uint num7 = (uint)((Math.Pow(2.0, 12.0) - 1.0) * (double)num6 / 10.0);
                    RTC5Wrap.n_write_da_x(this.Index + 1U, 2U, num7);
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
        public bool CtlWriteExtDO16(ushort bitPosition, bool onOff)
        {
            ushort num1 = (ushort)(1U << (int)bitPosition);
            ushort num2 = onOff ? num1 : (ushort)0;
            RTC5Wrap.n_write_io_port_mask(this.Index + 1U, (uint)num2, (uint)num1);
            this.stream?.WriteLine(string.Format("DATA : ExtDO16 : BIT{0} : {1}", (object)bitPosition, (object)onOff.ToString()));
            return true;
        }

        /// <summary>확장 포트에서 데이타 읽기</summary>
        /// <typeparam name="T">값(16비트, 8비트, 2비트 (uint), 아나로그(float 10V)</typeparam>
        /// <param name="ch">ExtDI2, ExtDI16, ExtDO16 가능</param>
        /// <param name="value">uint/float</param>
        /// <param name="compensator">compensator 보정용 객체</param>
        /// <returns></returns>
        public bool CtlReadData<T>(ExtensionChannel ch, out T value, ICompensator<T> compensator = null)
        {
            value = default(T);
            switch (ch)
            {
                case ExtensionChannel.ExtDI2:
                    uint num1 = RTC5Wrap.n_get_laser_pin_in(this.Index + 1U);
                    value = (T)Convert.ChangeType((object)num1, typeof(T));
                    break;
                case ExtensionChannel.ExtDO16:
                    uint num2 = RTC5Wrap.n_get_io_status(this.Index + 1U);
                    value = (T)Convert.ChangeType((object)num2, typeof(T));
                    break;
                case ExtensionChannel.ExtDI16:
                    uint num3 = RTC5Wrap.n_read_io_port(this.Index + 1U);
                    value = (T)Convert.ChangeType((object)num3, typeof(T));
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

        /// <summary>RTC5 내부 에러코드에 해당하는 메시지</summary>
        /// <param name="errorCode">에러코드</param>
        /// <returns></returns>
        public string CtlGetErrMsg(uint errorCode)
        {
            if (errorCode == 0U)
                return string.Empty;
            uint num = RTC5Wrap.n_get_error(this.Index + 1U);
            if (Convert.ToBoolean(num & 1U))
                return "no rtc board founded via init_rtc_dll";
            if (Convert.ToBoolean(num & 2U))
                return "access denied via init_rtc_dll, select, acquire_rtc";
            if (Convert.ToBoolean(num & 4U))
                return "command not forwarded. PCI or driver error";
            if (Convert.ToBoolean(num & 8U))
                return "rtc timed out. no response from board";
            if (Convert.ToBoolean(num & 16U))
                return "invalid parameter";
            if (Convert.ToBoolean(num & 32U))
                return "list processing is (not) active";
            if (Convert.ToBoolean(num & 64U))
                return "list command rejected, illegal input pointer";
            if (Convert.ToBoolean(num & 128U))
                return "list command wad converted to a List_mop";
            if (Convert.ToBoolean(num & 256U))
                return "dll, rtc or hex version error";
            if (Convert.ToBoolean(num & 512U))
                return "download verification error. load_program_file ?";
            if (Convert.ToBoolean(num & 1024U))
                return "DSP version is too old";
            if (Convert.ToBoolean(num & 2048U))
                return "out of memeory. dll internal windows memory request failed";
            if (Convert.ToBoolean(num & 4096U))
                return "EEPROM read or write error";
            return Convert.ToBoolean(num & 65536U) ? "error reading PCI configuration reqister druing init_rtc_dll" : string.Format("unknown error code : {0}", (object)errorCode);
        }

        /// <summary>RTC 카드의 상태 확인</summary>
        /// <param name="s">RtcStatus 열거형</param>
        /// <returns></returns>
        public bool CtlGetStatus(RtcStatus s)
        {
            bool flag1 = false;
            bool flag2 = false;
            switch (s)
            {
                case RtcStatus.Busy:
                    uint Status;
                    uint Pos;
                    RTC5Wrap.n_get_status(this.Index + 1U, out Status, out Pos);
                    bool flag3 = flag2 | Convert.ToBoolean(Status & 1U) | Convert.ToBoolean(Status & 128U) | Convert.ToBoolean(Status & 32768U);
                    flag1 = flag1 | flag3 | this.isManualOn;
                    break;
                case RtcStatus.NotBusy:
                    flag1 = !this.CtlGetStatus(RtcStatus.Busy);
                    break;
                case RtcStatus.List1Busy:
                    flag1 = Convert.ToBoolean(RTC5Wrap.n_read_status(this.Index + 1U) & 15U);
                    break;
                case RtcStatus.List2Busy:
                    flag1 = Convert.ToBoolean(RTC5Wrap.n_read_status(this.Index + 1U) & 16U);
                    break;
                case RtcStatus.NoError:
                    int num1 = this.CtlGetStatus(RtcStatus.Aborted) ? 1 : 0;
                    bool flag4 = RTC5Wrap.n_get_last_error(this.Index + 1U) > 0U;
                    flag1 = num1 == 0 && !flag4;
                    break;
                case RtcStatus.Aborted:
                    flag1 = this.isAborted;
                    break;
                case RtcStatus.PositionAckOK:
                    int num2 = (int)RTC5Wrap.n_get_head_status(this.Index + 1U, 1U);
                    flag1 = Convert.ToBoolean((uint)(num2 & 8)) & Convert.ToBoolean((uint)(num2 & 16));
                    break;
                case RtcStatus.PowerOK:
                    flag1 = Convert.ToBoolean(RTC5Wrap.n_get_head_status(this.Index + 1U, 1U) & 128U);
                    break;
                case RtcStatus.TempOK:
                    flag1 = Convert.ToBoolean(RTC5Wrap.n_get_head_status(this.Index + 1U, 1U) & 64U);
                    break;
            }
            return flag1;
        }

        /// <summary>리스트 명령이 완료될 때(busy 가 해제될때) 까지 대기하는 함수</summary>
        /// <returns></returns>
        public bool CtlBusyWait()
        {
            uint Status;
            do
            {
                Thread.Sleep(10);
                uint Pos;
                RTC5Wrap.n_get_status(this.Index + 1U, out Status, out Pos);
            }
            while (false | Convert.ToBoolean(Status & 1U) | Convert.ToBoolean(Status & 128U) | Convert.ToBoolean(Status & 32768U));
            return true;
        }

        /// <summary>실행중인 리스트 명령(busy 상태를)을 강제 종료</summary>
        /// <returns></returns>
        public bool CtlAbort()
        {
            RTC5Wrap.n_stop_execution(this.Index + 1U);
            this.isAborted = true;
            this.stream?.Flush();
            this.stream?.Dispose();
            this.stream = (StreamWriter)null;
            Logger.Log(Logger.Type.Warn, string.Format("rtc5 [{0}]: trying to abort !", (object)this.Index), Array.Empty<object>());
            return this.CtlGetStatus(RtcStatus.NotBusy);
        }

        /// <summary>에러상태를 해제</summary>
        /// <returns></returns>
        public bool CtlReset()
        {
            uint Code = RTC5Wrap.n_get_last_error(this.Index + 1U);
            if (Code != 0U)
                RTC5Wrap.n_reset_error(this.Index + 1U, Code);
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
                //this.rockey.InvalidLicense();
            if (!string.IsNullOrEmpty(this.outputFileName))
            {
                this.stream?.Dispose();
                this.stream = new StreamWriter(this.outputFileName);
            }
            this.listTempCount = 0U;
            this.listTotalCount = 0U;
            this.isAborted = false;
            this.listType = listType;
            this.listIndex = 1U;
            switch (this.listType)
            {
                case ListType.Single:
                    RTC5Wrap.n_config_list(this.Index + 1U, (uint)Math.Pow(2.0, 20.0) - this.RTC5_LIST3_BUFFER_SIZE, 0U);
                    break;
                case ListType.Auto:
                    RTC5Wrap.n_config_list(this.Index + 1U, this.RTC5_LIST_BUFFER_MAX * 2U, this.RTC5_LIST_BUFFER_MAX * 2U);
                    break;
            }
            RTC5Wrap.n_set_start_list(this.Index + 1U, this.listIndex);
            this.stream?.WriteLine("; LIST HAS BEGAN : " + DateTime.Now.ToString() + " with " + listType.ToString());
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: list has began ...", (object)this.Index), Array.Empty<object>());
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
            RTC5Wrap.n_set_laser_timing(this.Index + 1U, (uint)(num * 64.0), (uint)((double)pulseWidth * 64.0), (uint)((double)pulseWidth * 64.0), 0U);
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
            RTC5Wrap.n_set_scanner_delays(this.Index + 1U, (uint)((double)scannerJump / 10.0), (uint)((double)scannerMark / 10.0), (uint)((double)scannerPolygon / 10.0));
            RTC5Wrap.n_set_laser_delays(this.Index + 1U, (int)((double)laserOn * 2.0), (uint)((double)laserOff * 2.0));
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
            double Speed1 = (double)jump / 1000.0 * (double)this.KFactor;
            double Speed2 = (double)mark / 1000.0 * (double)this.KFactor;
            if (Speed1 < 1.6)
                Speed1 = 1.6;
            if (Speed1 > 800000.0)
                Speed1 = 800000.0;
            if (Speed2 < 1.6)
                Speed2 = 1.6;
            if (Speed2 > 800000.0)
                Speed2 = 800000.0;
            if (!this.IsListReady(2U))
                return false;
            RTC5Wrap.n_set_jump_speed(this.Index + 1U, Speed1);
            RTC5Wrap.n_set_mark_speed(this.Index + 1U, Speed2);
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
            uint count = (uint)((double)msec / 1000.0);
            float num = msec - (float)(count * 1000U);
            if (count > 0U)
            {
                if (!this.IsListReady(count))
                    return false;
                for (int index = 0; (long)index < (long)count; ++index)
                    RTC5Wrap.n_long_delay(this.Index + 1U, 100000U);
            }
            if ((double)num > 0.0)
            {
                if (!this.IsListReady(1U))
                    return false;
                RTC5Wrap.n_long_delay(this.Index + 1U, (uint)((double)num * 100.0));
            }
            this.stream?.WriteLine(string.Format("WAIT_DURING_MS = {0:F6}", (object)msec));
            return true;
        }

        /// <summary>리스트 명령 - 레이저 출사 시간</summary>
        /// <param name="msec">시간 (msec)</param>
        /// <returns></returns>
        public bool ListLaserOn(float msec)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            double num = (double)msec * 1000.0;
            RTC5Wrap.n_laser_on_list(this.Index + 1U, (uint)(num / 10.0));
            this.stream?.WriteLine(string.Format("LASER_ON_DURING_MS = {0:F6}", (object)msec));
            return true;
        }

        /// <summary>리스트 명령 - 레이저 출사 시작</summary>
        /// <returns></returns>
        public bool ListLaserOn()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            RTC5Wrap.n_laser_signal_on_list(this.Index + 1U);
            this.stream?.WriteLine("LASER_ON");
            return true;
        }

        /// <summary>리스트 명령 - 레이저 출사 중지</summary>
        /// <returns></returns>
        public bool ListLaserOff()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            RTC5Wrap.n_laser_signal_off_list(this.Index + 1U);
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
            int X = (int)((double)vector2.X * (double)this.KFactor);
            int Y = (int)((double)vector2.Y * (double)this.KFactor);
            if (!this.IsListReady(1U))
                return false;
            if (!this.isListAlcByVectorBegin)
            {
                RTC5Wrap.n_jump_abs(this.Index + 1U, X, Y);
                this.stream?.WriteLine(string.Format("JUMP_TO = {0:F3}, {1:F3}", (object)vector2.X, (object)vector2.Y));
            }
            else
            {
                RTC5Wrap.n_para_jump_abs(this.Index + 1U, X, Y, (uint)((double)this.valueByVector * (double)rampFactor));
                this.stream?.WriteLine(string.Format("JUMP_TO = {0:F3}, {1:F3}, RAMP={2:F3}", (object)vector2.X, (object)vector2.Y, (object)rampFactor));
            }
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
            int X = (int)((double)vector2.X * (double)this.KFactor);
            int Y = (int)((double)vector2.Y * (double)this.KFactor);
            if (!this.IsListReady(1U))
                return false;
            if (!this.isListAlcByVectorBegin)
            {
                RTC5Wrap.n_mark_abs(this.Index + 1U, X, Y);
                this.stream?.WriteLine(string.Format("MARK_TO = {0:F3}, {1:F3}", (object)vector2.X, (object)vector2.Y));
            }
            else
            {
                RTC5Wrap.n_para_jump_abs(this.Index + 1U, X, Y, (uint)((double)this.valueByVector * (double)rampFactor));
                this.stream?.WriteLine(string.Format("MARK_TO = {0:F3}, {1:F3}, RAMP={2:F3}", (object)vector2.X, (object)vector2.Y, (object)rampFactor));
            }
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

        public bool ListJumpWithDrill(Vector2 vPosition, uint dwellTime)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if (this.IsDuplicated(vPosition))
                return true;
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            int X = (int)((double)vector2.X * (double)this.KFactor);
            int Y = (int)((double)vector2.Y * (double)this.KFactor);
            if (!this.IsListReady(1U))
                return false;
            RTC5Wrap.n_jump_abs_drill(this.Index + 1U, X, Y, dwellTime / 10U);
            this.stream?.WriteLine(string.Format("JUMP_WITH_DRILL = {0:F3}, {1:F3}: {2}us", (object)vector2.X, (object)vector2.Y, (object)dwellTime));
            this.vPhysical = vector2;
            this.vLogical = vPosition;
            return true;
        }

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
                RTC5Wrap.n_arc_abs(this.Index + 1U, (int)((double)vector2.X * (double)this.KFactor), (int)((double)vector2.Y * (double)this.KFactor), (double)Math.Sign(sweepAngle) * -360.0);
            RTC5Wrap.n_arc_abs(this.Index + 1U, (int)((double)vector2.X * (double)this.KFactor), (int)((double)vector2.Y * (double)this.KFactor), -num2);
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
            Vector2 vector2 = Vector2.Transform(vCenter, this.MatrixStack.ToResult);
            if (!this.IsListReady(2U))
                return false;
            RTC5Wrap.n_set_ellipse(this.Index + 1U, (uint)((double)majorHalf * (double)this.KFactor), (uint)((double)minorHalf * (double)this.KFactor), -(double)startAngle, -(double)sweepAngle);
            RTC5Wrap.n_mark_ellipse_abs(this.Index + 1U, (int)((double)vector2.X * (double)this.KFactor), (int)((double)vector2.Y * (double)this.KFactor), (double)rotateAngle);
            this.stream?.WriteLine(string.Format("ELLIPSE_BY_CENTER = {0:F3}, {1:F3}, MAJOR = {2:F3}, MINOR = {3:F3}, START_ANGLE = {4:F3}, SWEEP_ANGLE = {5:F3}, ANGLE = {6:F3}", (object)vector2.X, (object)vector2.Y, (object)majorHalf, (object)minorHalf, (object)startAngle, (object)sweepAngle, (object)rotateAngle));
            return true;
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
            RTC5Wrap.n_list_nop(this.Index + 1U);
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
                    uint Pins = (uint)Convert.ChangeType((object)value, typeof(uint));
                    RTC5Wrap.n_set_laser_pin_out_list(this.Index + 1U, Pins);
                    break;
                case ExtensionChannel.ExtDO8:
                    uint num1 = (uint)Convert.ChangeType((object)value, typeof(uint));
                    RTC5Wrap.n_write_8bit_port_list(this.Index + 1U, num1);
                    break;
                case ExtensionChannel.ExtDO16:
                    uint num2 = (uint)Convert.ChangeType((object)value, typeof(uint));
                    RTC5Wrap.n_write_io_port_list(this.Index + 1U, num2);
                    break;
                case ExtensionChannel.ExtAO1:
                    double num3 = (double)Convert.ChangeType((object)value, typeof(double));
                    double num4 = (Math.Pow(2.0, 12.0) - 1.0) * num3 / 10.0;
                    RTC5Wrap.n_write_da_x_list(this.Index + 1U, 1U, (uint)num4);
                    break;
                case ExtensionChannel.ExtAO2:
                    double num5 = (double)Convert.ChangeType((object)value, typeof(double));
                    double num6 = (Math.Pow(2.0, 12.0) - 1.0) * num5 / 10.0;
                    RTC5Wrap.n_write_da_x_list(this.Index + 1U, 2U, (uint)num6);
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
        public bool ListWriteExtDO16(ushort bitPosition, bool onOff)
        {
            if (!this.IsListReady(1U))
                return false;
            RTC5Wrap.n_list_nop(this.Index + 1U);
            ushort num1 = (ushort)(1U << (int)bitPosition);
            ushort num2 = onOff ? num1 : (ushort)0;
            RTC5Wrap.n_write_io_port_mask_list(this.Index + 1U, (uint)num2, (uint)num1);
            this.stream?.WriteLine(string.Format("DATA : ExtDO16 : BIT{0} : {1}", (object)bitPosition, (object)onOff.ToString()));
            return true;
        }

        /// <summary>리스트 명령 끝 - 버퍼 닫기</summary>
        /// <returns></returns>
        /// s
        public bool ListEnd()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(2U))
                return false;
            RTC5Wrap.n_set_end_of_list(this.Index + 1U);
            this.stream?.WriteLine("; LIST ENDED : " + DateTime.Now.ToString());
            this.stream?.WriteLine(Environment.NewLine);
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: list has ended. counts= {1}", (object)this.Index, (object)this.listTotalCount), Array.Empty<object>());
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
                    RTC5Wrap.n_execute_list(this.Index + 1U, this.listIndex);
                    break;
                case ListType.Auto:
                    uint Status;
                    uint Pos;
                    RTC5Wrap.n_get_status(this.Index + 1U, out Status, out Pos);
                    if ((0 | (Convert.ToBoolean(Status & 1U) ? 1 : 0) | (Convert.ToBoolean(Status & 128U) ? 1 : 0) | (Convert.ToBoolean(Status & 32768U) ? 1 : 0)) != 0)
                    {
                        RTC5Wrap.n_auto_change(this.Index + 1U);
                        break;
                    }
                    RTC5Wrap.n_execute_list(this.Index + 1U, this.listIndex);
                    break;
            }
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: list executing", (object)this.Index), Array.Empty<object>());
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
                    return (double)this.listTotalCount < Math.Pow(2.0, 20.0) - (double)this.RTC5_LIST3_BUFFER_SIZE;
                case ListType.Auto:
                    if (this.listTempCount + count >= this.RTC5_LIST_BUFFER_MAX)
                    {
                        uint Status;
                        uint Pos;
                        RTC5Wrap.n_get_status(this.Index + 1U, out Status, out Pos);
                        if ((0 | (Convert.ToBoolean(Status & 1U) ? 1 : 0) | (Convert.ToBoolean(Status & 128U) ? 1 : 0) | (Convert.ToBoolean(Status & 32768U) ? 1 : 0)) == 0)
                        {
                            RTC5Wrap.n_set_end_of_list(this.Index + 1U);
                            RTC5Wrap.n_execute_list(this.Index + 1U, this.listIndex);
                            this.listIndex ^= 3U;
                            RTC5Wrap.n_set_start_list(this.Index + 1U, this.listIndex);
                        }
                        else
                        {
                            RTC5Wrap.n_set_end_of_list(this.Index + 1U);
                            if (this.CtlGetStatus(RtcStatus.Aborted))
                                return false;
                            RTC5Wrap.n_auto_change(this.Index + 1U);
                            switch (this.listIndex)
                            {
                                case 1:
                                    uint num1;
                                    do
                                    {
                                        num1 = RTC5Wrap.n_read_status(this.Index + 1U);
                                        Thread.Sleep(10);
                                    }
                                    while (Convert.ToBoolean(num1 & 32U));
                                    break;
                                case 2:
                                    uint num2;
                                    do
                                    {
                                        num2 = RTC5Wrap.n_read_status(this.Index + 1U);
                                        Thread.Sleep(10);
                                    }
                                    while (Convert.ToBoolean(num2 & 16U));
                                    break;
                            }
                            if (this.CtlGetStatus(RtcStatus.Aborted))
                                return false;
                            this.listIndex ^= 3U;
                            RTC5Wrap.n_set_start_list(this.Index + 1U, this.listIndex);
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
            int XHome = (int)((double)vector2.X * (double)this.KFactor);
            int YHome = (int)((double)vector2.Y * (double)this.KFactor);
            RTC5Wrap.n_home_position(this.Index + 1U, XHome, YHome);
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: home position: {1:F3}, {2:F3}", (object)this.Index, (object)vector2.X, (object)vector2.Y), Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 위치를 홈(Home) 위치로 설정</summary>
        /// <param name="x">x mm</param>
        /// <param name="y">y mm</param>
        /// <returns></returns>
        public bool CtlHomePosition(float x, float y) => this.CtlHomePosition(new Vector2(x, y));

        /// <summary>FPK(First Pulse Killer) 시간 설정</summary>
        /// <param name="usec">usec</param>
        /// <returns></returns>
        public bool CtlFirstPulseKiller(float usec)
        {
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            RTC5Wrap.n_set_firstpulse_killer(this.Index + 1U, (uint)((double)usec * 64.0));
            this.Fpk = usec;
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: fpk= {1:F1} usec", (object)this.Index, (object)usec), Array.Empty<object>());
            return true;
        }

        /// <summary>레이저 출력 신호 레벨 설정</summary>
        /// <param name="laserControlSignal">RTC 모델에 맞는 ILaserControlSignal 구현된 인스턴스</param>
        /// <returns></returns>
        public bool CtlLaserSignalLevel(ILaserControlSignal laserControlSignal)
        {
            RTC5Wrap.n_set_laser_control(this.Index + 1U, laserControlSignal.ToUInt());
            return true;
        }

        /// <summary>외부 트리거 사용시 설정</summary>
        /// <param name="mode">RTC 15핀 입력으로 /START, /STOP 등의 트리거 사용여부 설정</param>
        /// <param name="maxStartCounts">/START 트리거 최대 허용 개수 설정</param>
        /// <returns></returns>
        public bool CtlExternalControl(IRtcExternalControlMode mode, uint maxStartCounts = 0)
        {
            RTC5Wrap.n_set_control_mode(this.Index + 1U, mode.ToUInt());
            RTC5Wrap.n_set_max_counts(this.Index + 1U, maxStartCounts);
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: external control= {1}, max= {2}", (object)this.Index, (object)mode.ToUInt(), (object)maxStartCounts), Array.Empty<object>());
            return true;
        }

        /// <summary>외부 /START 실행된 회수 조회</summary>
        /// <param name="counts">회수값</param>
        /// <returns></returns>
        public bool CtlExternalStartCounts(out uint counts)
        {
            counts = RTC5Wrap.n_get_counts(this.Index + 1U);
            return true;
        }

        /// <summary>FPK(First Pulse Killer) 시간값 설정</summary>
        /// <param name="usec">usec</param>
        /// <returns></returns>
        public bool ListFirstPulseKiller(float usec)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            RTC5Wrap.n_set_firstpulse_killer_list(this.Index + 1U, (uint)((double)usec * 64.0));
            this.Fpk = usec;
            this.stream?.WriteLine(string.Format("FIRST PULSE KILLER = {0:F3} usec", (object)usec));
            return true;
        }

        /// <summary>
        /// 리스트 명령 - 레이저 가감속 구간의 모션 지연으로 인한 레이저 펄스의 중첩을 예방하기 위한 sky-writing 모드 사용
        /// </summary>
        /// <param name="laserOnShift">usec</param>
        /// <param name="timeLag">usec</param>
        /// <param name="angularLimit">활성화될 각도 설정 (예: 90도)</param>
        /// <returns></returns>
        public bool ListSkyWriting(float laserOnShift, float timeLag, float angularLimit)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(3U))
                return false;
            if ((double)timeLag == 0.0 && (double)laserOnShift == 0.0)
            {
                RTC5Wrap.n_set_sky_writing_mode_list(this.Index + 1U, 0U);
                RTC5Wrap.n_set_sky_writing_list(this.Index + 1U, 0.0, 0);
                double d = 1.57079631462693;
                RTC5Wrap.n_set_sky_writing_limit_list(this.Index + 1U, Math.Cos(d));
            }
            else
            {
                RTC5Wrap.n_set_sky_writing_mode_list(this.Index + 1U, 3U);
                RTC5Wrap.n_set_sky_writing_list(this.Index + 1U, (double)timeLag, (int)((double)laserOnShift * 2.0));
                double d = Math.PI / 180.0 * (double)angularLimit;
                RTC5Wrap.n_set_sky_writing_limit_list(this.Index + 1U, Math.Cos(d));
            }
            return true;
        }

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
            switch (ext)
            {
                case ExtensionChannel.ExtAO1:
                    RTC5Wrap.n_set_pixel_line(this.Index + 1U, 1U, (uint)((double)usec / 2.0 * 64.0), (double)num1, (double)num2);
                    break;
                case ExtensionChannel.ExtAO2:
                    RTC5Wrap.n_set_pixel_line(this.Index + 1U, 2U, (uint)((double)usec / 2.0 * 64.0), (double)num1, (double)num2);
                    break;
                default:
                    Logger.Log(Logger.Type.Warn, string.Format("rtc5 [{0}]: {1} is not supported in ListPixelLine", (object)this.Index, (object)ext), Array.Empty<object>());
                    return false;
            }
            this.pixelExtensionChannel = ext;
            return true;
        }

        /// <summary>
        /// 리스트 명령 - 개별 픽셀 명령
        /// 반드시 ListPixelLine 명령이 호출된후에 픽셀 개수만큼의 ListPixel 함수가 호출되어야 함
        /// </summary>
        /// <param name="usec">현재 픽셀의 출력 주기(lower than usec in ListPixelLine ) </param>
        /// <param name="weight">ExtensionChannel 출력의 가중치 값(0~1), 내부적으로는 float : 아나로그 10V</param>
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
                    uint AnalogOut = (uint)((Math.Pow(2.0, 12.0) - 1.0) * (double)weight);
                    RTC5Wrap.n_set_pixel(this.Index + 1U, (uint)((double)usec * 64.0), AnalogOut);
                    return true;
                default:
                    Logger.Log(Logger.Type.Warn, string.Format("rtc5 [{0}]: {1} is not supported in ListPixelLine", (object)this.Index, (object)this.pixelExtensionChannel), Array.Empty<object>());
                    return false;
            }
        }

        /// <summary>리스트 명령 - 위 ListPixelLine + ListPixel * n 을 통합한 편이용 함수</summary>
        /// <param name="start">가공 시작점</param>
        /// <param name="end">가공 끝점</param>
        /// <param name="periodUsec">픽셀 주기 (usec)</param>
        /// <param name="usecValues">매 픽셀 가공 시간 배열</param>
        /// <param name="ext">아나로그 확장 출력 1,2 지정</param>
        /// <param name="analogValues">아나로그 값(0~10) 배열</param>
        /// <param name="usecCompensator">픽셀 출력시간값 보정기 사용시</param>
        /// <param name="analogCompensator">아나로그 출력 보정기 사용시</param>
        /// <returns></returns>
        public bool ListPixels(
          Vector2 start,
          Vector2 end,
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
            bool flag = true & this.ListJump(start, 1f);
            Vector2 position;
            position.X = (end.X - start.X) / (float)length;
            position.Y = (end.Y - start.Y) / (float)length;
            if (!this.IsListReady((uint)(2 + length)))
                return false;
            Matrix3x2 toResult = this.MatrixStack.ToResult;
            toResult.M31 = 0.0f;
            toResult.M32 = 0.0f;
            Vector2 vector2 = Vector2.Transform(position, toResult);
            int num1 = (int)((double)vector2.X * (double)this.KFactor);
            int num2 = (int)((double)vector2.Y * (double)this.KFactor);
            switch (ext)
            {
                case ExtensionChannel.ExtAO1:
                    RTC5Wrap.n_set_pixel_line(this.Index + 1U, 1U, (uint)((double)periodUsec / 2.0 * 64.0), (double)num1, (double)num2);
                    break;
                case ExtensionChannel.ExtAO2:
                    RTC5Wrap.n_set_pixel_line(this.Index + 1U, 2U, (uint)((double)periodUsec / 2.0 * 64.0), (double)num1, (double)num2);
                    break;
                default:
                    Logger.Log(Logger.Type.Warn, string.Format("rtc5 [{0}]: {1} is not supported in ListPixelLine", (object)this.Index, (object)ext), Array.Empty<object>());
                    break;
            }
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
                uint AnalogOut = (uint)((Math.Pow(2.0, 12.0) - 1.0) * (double)input2 / 10.0);
                RTC5Wrap.n_set_pixel(this.Index + 1U, (uint)((double)input1 * 64.0), AnalogOut);
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
            uint Longitudinal = (uint)((double)amplitudeX * (double)this.KFactor);
            uint Transversal = (uint)((double)amplitudeY * (double)this.KFactor);
            RTC5Wrap.n_set_wobbel(this.Index + 1U, Transversal, Longitudinal, (double)this.frequency);
            return true;
        }

        /// <summary>리스트 명령 - 하드 점프(Hard Jump)</summary>
        /// <param name="vPosition">x,y 위치</param>
        /// <param name="laserOn">usec</param>
        /// <param name="laserOff">usec</param>
        /// <returns></returns>
        public bool ListJumpHard(Vector2 vPosition, float laserOn, float laserOff)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if (this.IsDuplicated(vPosition))
                return true;
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            int X = (int)((double)vector2.X * (double)this.KFactor);
            int Y = (int)((double)vector2.Y * (double)this.KFactor);
            if (!this.IsListReady(1U))
                return false;
            RTC5Wrap.n_micro_vector_abs(this.Index + 1U, X, Y, (int)laserOn * 2, (int)laserOff * 2);
            this.vPhysical = vector2;
            this.vLogical = vPosition;
            this.stream?.WriteLine(string.Format("JUMP_HARD_TO = {0:F3}, {1:F3}", (object)vector2.X, (object)vector2.Y));
            return true;
        }

        /// <summary>리스트 명령 - 하드 점프(Hard Jump)</summary>
        /// <param name="x">x,y 위치</param>
        /// <param name="y">x,y 위치</param>
        /// <param name="laserOn">usec</param>
        /// <param name="laserOff">usec</param>
        /// <returns></returns>
        public bool ListJumpHard(float x, float y, float laserOn, float laserOff) => this.ListJumpHard(new Vector2(x, y), laserOn, laserOff);

        /// <summary>측정된 데이타 가져오기</summary>
        /// <param name="channel">채널</param>
        /// <param name="data">데이타 배열</param>
        /// <returns></returns>
        public bool CtlGetMeasurement(MeasurementChannel channel, out int[] data)
        {
            data = (int[])null;
            uint Busy;
            uint Pos;
            RTC5Wrap.n_measurement_status(this.Index + 1U, out Busy, out Pos);
            if (Busy > 0U)
                return false;
            data = new int[(int)Pos];
            RTC5Wrap.n_get_waveform(this.Index + 1U, (uint)channel, Pos, data);
            return true;
        }

        /// <summary>리스트 명령 - 측정 시작</summary>
        /// <param name="frequency">usec</param>
        /// <param name="channels">대상 채널</param>
        /// <returns></returns>
        public bool ListMeasurementBegin(float frequency, MeasurementChannel[] channels)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if (channels.Length != 4)
            {
                Logger.Log(Logger.Type.Error, "measurement count are mismatched (should be 4)", Array.Empty<object>());
                return false;
            }
            if (!this.IsListReady(1U))
                return false;
            double num = 1.0 / (double)frequency * 1000000.0;
            RTC5Wrap.n_set_trigger4(this.Index + 1U, (uint)(num / 10.0), (uint)channels[0], (uint)channels[1], (uint)channels[2], (uint)channels[3]);
            return true;
        }

        /// <summary>리스트 명령 - 측정 끝</summary>
        /// <returns></returns>
        public bool ListMeasurementEnd()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(2U))
                return false;
            RTC5Wrap.n_set_trigger(this.Index + 1U, 0U, 0U, 0U);
            RTC5Wrap.n_set_trigger4(this.Index + 1U, 0U, 0U, 0U, 0U, 0U);
            return true;
        }

        /// <summary>ALC(Automatic Laser Control) 기능 설정</summary>
        /// <typeparam name="T">AutoLaserControlSignal 열거형중 ExtDO 는 uint, 그외는 float</typeparam>
        /// <param name="ctrl">AutoLaserControlSignal 열거형</param>
        /// <param name="mode">AutoLaserControlMode 열거형</param>
        /// <param name="percentage100">100% 일때의 출력값</param>
        /// <param name="min">최소 출력값</param>
        /// <param name="max">최대 출력값</param>
        /// <param name="compensator">보정기</param>
        /// <returns></returns>
        public bool CtlAutoLaserControl<T>(
          AutoLaserControlSignal ctrl,
          AutoLaserControlMode mode,
          T percentage100,
          T min,
          T max,
          ICompensator<T> compensator = null)
        {
            if (compensator != null)
            {
                T output1;
                T output2;
                T output3;
                if (!compensator.Interpolate(percentage100, out output1) || !compensator.Interpolate(min, out output2) || !compensator.Interpolate(max, out output3))
                    return false;
                percentage100 = output1;
                min = output2;
                max = output3;
            }
            if (string.IsNullOrEmpty(this.AutoLaserControlByPositionFileName) || !File.Exists(this.AutoLaserControlByPositionFileName))
            {
                int num = RTC5Wrap.n_load_position_control(this.Index + 1U, (string)null, 0U);
                Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control by position dependent is disabled", (object)this.Index), Array.Empty<object>());
            }
            else
            {
                int num = RTC5Wrap.n_load_position_control(this.Index + 1U, this.AutoLaserControlByPositionFileName, this.AutoLaserControlByPositionTableNo);
                if (num <= -1 && num >= -50)
                {
                    Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control by position dependent: {1} [{2}]: {3} points", (object)this.Index, (object)this.AutoLaserControlByPositionFileName, (object)this.AutoLaserControlByPositionTableNo, (object)Math.Abs(num)), Array.Empty<object>());
                }
                else
                {
                    switch (num)
                    {
                        case 1:
                            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control: {1}: No valid data points found(though Table No found", (object)this.Index, (object)this.AutoLaserControlByPositionFileName), Array.Empty<object>());
                            break;
                        case 3:
                            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control: {1}: File not founded", (object)this.Index, (object)this.AutoLaserControlByPositionFileName), Array.Empty<object>());
                            break;
                        case 4:
                            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control: {1}: DSP memory error", (object)this.Index, (object)this.AutoLaserControlByPositionFileName), Array.Empty<object>());
                            break;
                        case 5:
                            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control: {1}: BUSY error, board was BUSY or INTERNAL-BUSY", (object)this.Index, (object)this.AutoLaserControlByPositionFileName), Array.Empty<object>());
                            break;
                        case 8:
                            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control: {1}: Board is locked by another user program", (object)this.Index, (object)this.AutoLaserControlByPositionFileName), Array.Empty<object>());
                            break;
                        case 11:
                            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control: {1}: PCI error", (object)this.Index, (object)this.AutoLaserControlByPositionFileName), Array.Empty<object>());
                            break;
                        case 13:
                            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control: {1}: The specified table number was not found in the file", (object)this.Index, (object)this.AutoLaserControlByPositionFileName), Array.Empty<object>());
                            break;
                        default:
                            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control: {1}: unknown error", (object)this.Index, (object)this.AutoLaserControlByPositionFileName), Array.Empty<object>());
                            break;
                    }
                    return false;
                }
            }
            return this.CtlAlcSet<T>(ctrl, mode, percentage100, min, max);
        }

        protected bool CtlAlcSet<T>(
          AutoLaserControlSignal ctrl,
          AutoLaserControlMode mode,
          T percentage100,
          T min,
          T max,
          ICompensator<T> compensator = null)
        {
            uint num1 = 0;
            switch (ctrl)
            {
                case AutoLaserControlSignal.Disabled:
                    num1 = RTC5Wrap.n_set_auto_laser_control(this.Index + 1U, (uint)ctrl, 0U, (uint)mode, 0U, 0U);
                    break;
                case AutoLaserControlSignal.Analog1:
                case AutoLaserControlSignal.Analog2:
                    float num2 = (float)Convert.ChangeType((object)percentage100, typeof(float));
                    float num3 = (float)Convert.ChangeType((object)min, typeof(float));
                    float num4 = (float)Convert.ChangeType((object)max, typeof(float));
                    uint num5 = (uint)((Math.Pow(2.0, 12.0) - 1.0) * (double)num2 / 10.0);
                    uint MinValue1 = (uint)((Math.Pow(2.0, 12.0) - 1.0) * (double)num3 / 10.0);
                    uint MaxValue1 = (uint)((Math.Pow(2.0, 12.0) - 1.0) * (double)num4 / 10.0);
                    num1 = RTC5Wrap.n_set_auto_laser_control(this.Index + 1U, (uint)ctrl, num5, (uint)mode, MinValue1, MaxValue1);
                    break;
                case AutoLaserControlSignal.ExtDO8Bit:
                    uint num6 = (uint)Convert.ChangeType((object)percentage100, typeof(uint));
                    uint MinValue2 = (uint)Convert.ChangeType((object)min, typeof(uint));
                    uint MaxValue2 = (uint)Convert.ChangeType((object)max, typeof(uint));
                    num1 = RTC5Wrap.n_set_auto_laser_control(this.Index + 1U, (uint)ctrl, num6, (uint)mode, MinValue2, MaxValue2);
                    break;
                case AutoLaserControlSignal.PulseWidth:
                    float num7 = (float)Convert.ChangeType((object)percentage100, typeof(float));
                    float num8 = (float)Convert.ChangeType((object)min, typeof(float));
                    float num9 = (float)Convert.ChangeType((object)max, typeof(float));
                    num1 = RTC5Wrap.n_set_auto_laser_control(this.Index + 1U, (uint)ctrl, (uint)((double)num7 * 64.0), (uint)mode, (uint)((double)num8 * 64.0), (uint)((double)num9 * 64.0));
                    break;
                case AutoLaserControlSignal.Frequency:
                    double num10 = 1.0 / (double)(float)Convert.ChangeType((object)percentage100, typeof(float)) * 1000000.0 / 2.0;
                    double num11 = 1.0 / (double)(float)Convert.ChangeType((object)min, typeof(float)) * 1000000.0 / 2.0;
                    double num12 = 1.0 / (double)(float)Convert.ChangeType((object)max, typeof(float)) * 1000000.0 / 2.0;
                    num1 = RTC5Wrap.n_set_auto_laser_control(this.Index + 1U, (uint)ctrl, (uint)(num10 * 64.0), (uint)mode, (uint)(num11 * 64.0), (uint)(num12 * 64.0));
                    break;
                case AutoLaserControlSignal.ExtDO16:
                    uint num13 = (uint)Convert.ChangeType((object)percentage100, typeof(uint));
                    uint MinValue3 = (uint)Convert.ChangeType((object)min, typeof(uint));
                    uint MaxValue3 = (uint)Convert.ChangeType((object)max, typeof(uint));
                    num1 = RTC5Wrap.n_set_auto_laser_control(this.Index + 1U, (uint)ctrl, num13, (uint)mode, MinValue3, MaxValue3);
                    break;
            }
            if (mode == AutoLaserControlMode.Disabled)
                Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control : {1} disabled", (object)this.Index, (object)ctrl), Array.Empty<object>());
            switch (num1)
            {
                case 0:
                    Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: automatic laser control : {1}, {2}, 100%={3}, min={4}, max={5}, ret={6}", (object)this.Index, (object)mode, (object)ctrl, (object)percentage100, (object)min, (object)max, (object)num1), Array.Empty<object>());
                    return true;
                case 1:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: automatic laser control : no primary scan head active", (object)this.Index), Array.Empty<object>());
                    break;
                case 2:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: automatic laser control : no iDrive scan system", (object)this.Index), Array.Empty<object>());
                    break;
                case 3:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: automatic laser control : invalid {1} value", (object)this.Index, (object)ctrl), Array.Empty<object>());
                    break;
                case 4:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: automatic laser control : invalid {1} value", (object)this.Index, (object)mode), Array.Empty<object>());
                    break;
                case 5:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: automatic laser control : access denied", (object)this.Index), Array.Empty<object>());
                    break;
                default:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: automatic laser control : unknown error: {1}", (object)this.Index, (object)num1), Array.Empty<object>());
                    break;
            }
            return false;
        }

        /// <summary>
        /// 리스트 명령 - ALC(Automatic Laser Control) 기능중 Vector Dependent 기능을 활성화
        /// </summary>
        /// <typeparam name="T">AutoLaserControlSignal 열거형중 ExtDO 는 uint, 그외는 float</typeparam>
        /// <param name="ctrl">AutoLaserControlSignal 열거형</param>
        /// <param name="startingValue">시작 출력값</param>
        /// <param name="compensator">보정기</param>
        /// <returns></returns>
        public bool ListAlcByVectorBegin<T>(
          AutoLaserControlSignal ctrl,
          T startingValue,
          ICompensator<T> compensator = null)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(2U))
                return false;
            if (compensator != null)
            {
                T output;
                if (!compensator.Interpolate(startingValue, out output))
                    return false;
                startingValue = output;
            }
            switch (ctrl)
            {
                case AutoLaserControlSignal.Analog1:
                case AutoLaserControlSignal.Analog2:
                    float num1 = (float)Convert.ChangeType((object)startingValue, typeof(float));
                    uint num2 = (uint)((Math.Pow(2.0, 12.0) - 1.0) * (double)num1 / 10.0);
                    RTC5Wrap.n_set_vector_control(this.Index + 1U, (uint)ctrl, num2);
                    this.valueByVector = num2;
                    StreamWriter stream1 = this.stream;
                    if (stream1 != null)
                    {
                        stream1.WriteLine(string.Format("AUTO LASER CONTROL BY VECTOR : {0}= {1:F3}V", (object)ctrl, (object)startingValue));
                        break;
                    }
                    break;
                case AutoLaserControlSignal.ExtDO8Bit:
                    uint num3 = (uint)Convert.ChangeType((object)startingValue, typeof(uint));
                    RTC5Wrap.n_set_vector_control(this.Index + 1U, (uint)ctrl, num3);
                    this.valueByVector = num3;
                    StreamWriter stream2 = this.stream;
                    if (stream2 != null)
                    {
                        stream2.WriteLine(string.Format("AUTO LASER CONTROL BY VECTOR : {0}= {1:X}", (object)ctrl, (object)startingValue));
                        break;
                    }
                    break;
                case AutoLaserControlSignal.PulseWidth:
                    float num4 = (float)Convert.ChangeType((object)startingValue, typeof(float));
                    RTC5Wrap.n_set_vector_control(this.Index + 1U, (uint)ctrl, (uint)((double)num4 * 64.0));
                    this.valueByVector = (uint)((double)num4 * 64.0);
                    StreamWriter stream3 = this.stream;
                    if (stream3 != null)
                    {
                        stream3.WriteLine(string.Format("AUTO LASER CONTROL BY VECTOR : {0}= {1:F3}usec", (object)ctrl, (object)startingValue));
                        break;
                    }
                    break;
                case AutoLaserControlSignal.Frequency:
                    double num5 = 1.0 / (double)(float)Convert.ChangeType((object)startingValue, typeof(float)) * 1000000.0 / 2.0;
                    RTC5Wrap.n_set_vector_control(this.Index + 1U, (uint)ctrl, (uint)(num5 * 64.0));
                    this.valueByVector = (uint)(num5 * 64.0);
                    StreamWriter stream4 = this.stream;
                    if (stream4 != null)
                    {
                        stream4.WriteLine(string.Format("AUTO LASER CONTROL BY VECTOR : {0}= {1:F3}Hz", (object)ctrl, (object)startingValue));
                        break;
                    }
                    break;
                case AutoLaserControlSignal.ExtDO16:
                    uint num6 = (uint)Convert.ChangeType((object)startingValue, typeof(uint));
                    RTC5Wrap.n_set_vector_control(this.Index + 1U, (uint)ctrl, num6);
                    this.valueByVector = num6;
                    StreamWriter stream5 = this.stream;
                    if (stream5 != null)
                    {
                        stream5.WriteLine(string.Format("AUTO LASER CONTROL BY VECTOR : {0}= {1:X}", (object)ctrl, (object)startingValue));
                        break;
                    }
                    break;
            }
            this.isListAlcByVectorBegin = true;
            this.byVectorAlcControlSignal = ctrl;
            return true;
        }

        /// <summary>
        /// 리스트 명령 -  ALC(Automatic Laser Control) 기능중 Vector Dependent 기능을 비활성화
        /// </summary>
        /// <returns></returns>
        public bool ListAlcByVectorEnd()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(2U))
                return false;
            RTC5Wrap.n_set_vector_control(this.Index + 1U, (uint)this.byVectorAlcControlSignal, 0U);
            this.valueByVector = 0U;
            this.isListAlcByVectorBegin = false;
            this.stream?.WriteLine("AUTO LASER CONTROL BY VECTOR : DISABLED");
            return true;
        }

        /// <summary>듀얼 헤드 사용시 개별 헤드에 대한 오프셋 이동 회전량 설정</summary>
        /// <param name="head">primary or secondary</param>
        /// <param name="offset">dx,dy (mm)</param>
        /// <param name="angle">회전 (각도)</param>
        /// <returns></returns>
        public bool CtlHeadOffset(ScannerHead head, Vector2 offset, float angle)
        {
            int XOffset = (int)((double)offset.X * (double)this.KFactor);
            int YOffset = (int)((double)offset.Y * (double)this.KFactor);
            switch (head)
            {
                case ScannerHead.Primary:
                    RTC5Wrap.n_set_angle(this.Index + 1U, 1U, (double)angle, 1U);
                    RTC5Wrap.n_set_offset_xyz(this.Index + 1U, 1U, XOffset, YOffset, 0, 1U);
                    this.PrimaryHeadOffset = offset;
                    this.PrimaryHeadAngle = angle;
                    Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: primary head offset : {1}, angle={2:F3}", (object)this.Index, (object)offset.ToString(), (object)angle), Array.Empty<object>());
                    break;
                case ScannerHead.Secondary:
                    RTC5Wrap.n_set_angle(this.Index + 1U, 2U, (double)angle, 1U);
                    RTC5Wrap.n_set_offset_xyz(this.Index + 1U, 2U, XOffset, YOffset, 0, 1U);
                    this.SecondaryHeadOffset = offset;
                    this.SecondaryHeadAngle = angle;
                    Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: secondary head offset : {1}, angle={2:F3}", (object)this.Index, (object)offset.ToString(), (object)angle), Array.Empty<object>());
                    break;
                default:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: fail to reset head offset", (object)this.Index), Array.Empty<object>());
                    return false;
            }
            return true;
        }

        /// <summary>리스트 명령 - 듀얼 헤드 사용시 개별 헤드에 대한 오프셋 이동 회전량 설정</summary>
        /// <param name="head">primary or secondary</param>
        /// <param name="offset">dx, dy (mm)</param>
        /// <param name="angle">회전 (각도)</param>
        /// <returns></returns>
        public bool ListHeadOffset(ScannerHead head, Vector2 offset, float angle)
        {
            int XOffset = (int)((double)offset.X * (double)this.KFactor);
            int YOffset = (int)((double)offset.Y * (double)this.KFactor);
            if (!this.IsListReady(2U))
                return false;
            switch (head)
            {
                case ScannerHead.Primary:
                    RTC5Wrap.n_set_angle_list(this.Index + 1U, 1U, (double)angle, 1U);
                    RTC5Wrap.n_set_offset_xyz_list(this.Index + 1U, 1U, XOffset, YOffset, 0, 1U);
                    this.PrimaryHeadOffset = offset;
                    this.PrimaryHeadAngle = angle;
                    break;
                case ScannerHead.Secondary:
                    RTC5Wrap.n_set_angle_list(this.Index + 1U, 2U, (double)angle, 1U);
                    RTC5Wrap.n_set_offset_xyz_list(this.Index + 1U, 2U, XOffset, YOffset, 0, 1U);
                    this.SecondaryHeadOffset = offset;
                    this.SecondaryHeadAngle = angle;
                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>스캐너 이동</summary>
        /// <param name="vPosition">x, y, z (mm)</param>
        /// <returns></returns>
        public bool CtlMove(Vector3 vPosition)
        {
            Matrix4x4 matrix = new Matrix4x4(this.MatrixStack.ToResult);
            Vector3 vector3 = Vector3.Transform(vPosition, matrix);
            int X = (int)((double)vector3.X * (double)this.KFactor);
            int Y = (int)((double)vector3.Y * (double)this.KFactor);
            int Z = (int)((double)vector3.Z * (double)this.KZFactor);
            RTC5Wrap.n_goto_xyz(this.Index + 1U, X, Y, Z);
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: scanner moved : {1:F3}, {2:F3}, {3:F3} +{4:F3}", (object)this.Index, (object)vector3.X, (object)vector3.Y, (object)vector3.Z, (object)this.ZOffset), Array.Empty<object>());
            this.vPhysical3D = vector3;
            this.vLogical3D = vPosition;
            return true;
        }

        /// <summary>Z 오프셋</summary>
        /// <param name="zOffset">포커스 Z 이동 오프셋 량 (mm))</param>
        /// <returns></returns>
        public bool CtlZOffset(float zOffset)
        {
            this.ZOffset = zOffset;
            int ZOffset = (int)((double)zOffset * (double)this.KZFactor);
            RTC5Wrap.n_set_offset_xyz(this.Index + 1U, 0U, 0, 0, ZOffset, 1U);
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: z offset = {1:F3}", (object)this.Index, (object)zOffset), Array.Empty<object>());
            return true;
        }

        /// <summary>Z 디포커스</summary>
        /// <param name="zDefocus">디포커스 Z 이동량 (mm)</param>
        /// <returns></returns>
        public bool CtlZDefocus(float zDefocus)
        {
            this.ZDefocus = zDefocus;
            int Shift = (int)((double)zDefocus * (double)this.KZFactor);
            RTC5Wrap.n_set_defocus(this.Index + 1U, Shift);
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: defocus = {1:F3}", (object)this.Index, (object)zDefocus), Array.Empty<object>());
            return true;
        }

        /// <summary>리스트 명령 - Z 오프셋</summary>
        /// <param name="zOffset">포커스 Z 이동 오프셋 량 (mm))</param>
        /// <returns></returns>
        public bool ListZOffset(float zOffset)
        {
            this.ZOffset = zOffset;
            int ZOffset = (int)((double)zOffset * (double)this.KZFactor);
            if (!this.IsListReady(1U))
                return false;
            RTC5Wrap.n_set_offset_xyz_list(this.Index + 1U, 0U, 0, 0, ZOffset, 1U);
            return true;
        }

        /// <summary>리스트 명령 - Z 디포커스</summary>
        /// <param name="zDefocus">디포커스 Z 이동량 (mm)</param>
        /// <returns></returns>
        public bool ListZDefocus(float zDefocus)
        {
            this.ZDefocus = zDefocus;
            int Shift = (int)((double)zDefocus * (double)this.KZFactor);
            if (!this.IsListReady(1U))
                return false;
            RTC5Wrap.n_set_defocus_list(this.Index + 1U, Shift);
            return true;
        }

        /// <summary>리스트 명령 - 점프</summary>
        /// <param name="vPosition">x,y,z 위치 (mm)</param>
        /// <param name="rampFactor">자동 레이저 제어시의 비율값</param>
        /// <returns></returns>
        public bool ListJump(Vector3 vPosition, float rampFactor = 1f)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if (this.IsDuplicated(vPosition))
                return true;
            Matrix4x4 matrix = new Matrix4x4(this.MatrixStack.ToResult);
            Vector3 vector3 = Vector3.Transform(vPosition, matrix);
            int X = (int)((double)vector3.X * (double)this.KFactor);
            int Y = (int)((double)vector3.Y * (double)this.KFactor);
            int Z = (int)((double)vPosition.Z * (double)this.KZFactor);
            if (!this.IsListReady(1U))
                return false;
            if (!this.isListAlcByVectorBegin)
            {
                RTC5Wrap.n_jump_abs_3d(this.Index + 1U, X, Y, Z);
                this.stream?.WriteLine(string.Format("JUMP_TO = {0:F3}, {1:F3}, {2:F3} (+{3:F3})", (object)vector3.X, (object)vector3.Y, (object)vector3.Z, (object)this.ZOffset));
            }
            else
            {
                RTC5Wrap.n_para_jump_abs_3d(this.Index + 1U, X, Y, Z, (uint)((double)this.valueByVector * (double)rampFactor));
                this.stream?.WriteLine(string.Format("JUMP_TO = {0:F3}, {1:F3}, {2:F3}, (+{3:F3}) RAMP={4:F3}", (object)vector3.X, (object)vector3.Y, (object)vector3.Z, (object)this.ZOffset, (object)rampFactor));
            }
            this.vPhysical3D = vector3;
            this.vLogical3D = vPosition;
            return true;
        }

        /// <summary>리스트 명령 - 점프</summary>
        /// <param name="x">x 위치 (mm)</param>
        /// <param name="y">y 위치 (mm)</param>
        /// <param name="z">z 위치 (mm)</param>
        /// <param name="rampFactor">자동 레이저 제어시의 비율값</param>
        /// <returns></returns>
        public bool ListJump(float x, float y, float z, float rampFactor = 1f) => this.ListJump(new Vector3(x, y, z), rampFactor);

        /// <summary>리스트 명령 - 마크</summary>
        /// <param name="vPosition">x,y,z 위치 (mm)</param>
        /// <param name="rampFactor">자동 레이저 제어시의 비율값</param>
        /// <returns></returns>
        public bool ListMark(Vector3 vPosition, float rampFactor = 1f)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if (this.IsDuplicated(vPosition))
                return true;
            Matrix4x4 matrix = new Matrix4x4(this.MatrixStack.ToResult);
            Vector3 vector3 = Vector3.Transform(vPosition, matrix);
            int X = (int)((double)vector3.X * (double)this.KFactor);
            int Y = (int)((double)vector3.Y * (double)this.KFactor);
            int Z = (int)((double)vPosition.Z * (double)this.KZFactor);
            if (!this.IsListReady(1U))
                return false;
            if (!this.isListAlcByVectorBegin)
            {
                RTC5Wrap.n_mark_abs_3d(this.Index + 1U, X, Y, Z);
                this.stream?.WriteLine(string.Format("MARK_TO = {0:F3}, {1:F3}, {2:F3} (+{3:F3})", (object)vector3.X, (object)vector3.Y, (object)vector3.Z, (object)this.ZOffset));
            }
            else
            {
                RTC5Wrap.n_para_mark_abs_3d(this.Index + 1U, X, Y, Z, (uint)((double)this.valueByVector * (double)rampFactor));
                this.stream?.WriteLine(string.Format("MARK_TO = {0:F3}, {1:F3}, {2:F3}, (+{3:F3}) RAMP={4:F3}", (object)vector3.X, (object)vector3.Y, (object)vector3.Z, (object)this.ZOffset, (object)rampFactor));
            }
            this.vPhysical3D = vector3;
            this.vLogical3D = vPosition;
            return true;
        }

        /// <summary>리스트 명령 - 마크</summary>
        /// <param name="x">x 위치 (mm)</param>
        /// <param name="y">y 위치 (mm)</param>
        /// <param name="z">z 위치 (mm)</param>
        /// <param name="rampFactor">자동 레이저 제어시의 비율값</param>
        /// <returns></returns>
        public bool ListMark(float x, float y, float z, float rampFactor = 1f) => this.ListMark(new Vector3(x, y, z), rampFactor);

        /// <summary>리스트 명령 - 아크(호)</summary>
        /// <param name="vCenter">중심 위치 (cx, cy, cz) (mm)</param>
        /// <param name="sweepAngle">회전 각도 (+ : 반시계방향)</param>
        /// <returns></returns>
        public bool ListArc(Vector3 vCenter, float sweepAngle)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            Matrix4x4 matrix = new Matrix4x4(this.MatrixStack.ToResult);
            Vector3 vector3 = Vector3.Transform(vCenter, matrix);
            int num1 = (int)((double)Math.Abs(sweepAngle) / 360.0);
            double num2 = (double)sweepAngle - (double)Math.Sign(sweepAngle) * 360.0 * (double)num1;
            if (!this.IsListReady((uint)(num1 + 1)))
                return false;
            for (int index = 0; index < num1; ++index)
                RTC5Wrap.n_arc_abs_3d(this.Index + 1U, (int)((double)vector3.X * (double)this.KFactor), (int)((double)vector3.Y * (double)this.KFactor), (int)(((double)this.ZOffset + (double)vector3.Z) * (double)this.KZFactor), (double)Math.Sign(sweepAngle) * -360.0);
            RTC5Wrap.n_arc_abs_3d(this.Index + 1U, (int)((double)vector3.X * (double)this.KFactor), (int)((double)vector3.Y * (double)this.KFactor), (int)(((double)this.ZOffset + (double)vector3.Z) * (double)this.KZFactor), -num2);
            double num3 = 0.0;
            if ((double)this.vLogical.Y != (double)vCenter.Y || (double)this.vLogical.X != (double)vCenter.Y)
                num3 = Math.Atan2((double)this.vLogical.Y - (double)vCenter.Y, (double)this.vLogical.X - (double)vCenter.X);
            double num4 = Math.Sqrt(((double)vCenter.X - (double)this.vLogical.X) * ((double)vCenter.X - (double)this.vLogical.X) + ((double)vCenter.Y - (double)this.vLogical.Y) * ((double)vCenter.Y - (double)this.vLogical.Y));
            this.vLogical3D.X = (float)(num4 * Math.Cos(num3 + Math.PI / 180.0 * (double)sweepAngle)) + vCenter.X;
            this.vLogical3D.Y = (float)(num4 * Math.Sin(num3 + Math.PI / 180.0 * (double)sweepAngle)) + vCenter.Y;
            this.vLogical3D.Z = vCenter.Z;
            this.vPhysical3D = Vector3.Transform(this.vLogical3D, matrix);
            this.stream?.WriteLine(string.Format("ARC_BY_CENTER = {0:F3}, {1:F3}, {2:F3} (+{3:F3}) SWEEP_ANGLE = {4:F3}", (object)vector3.X, (object)vector3.Y, (object)vector3.Z, (object)this.ZOffset, (object)sweepAngle));
            return true;
        }

        /// <summary>리스트 명령 - 아크(호)</summary>
        /// <param name="cx">중심 위치 (cx) (mm)</param>
        /// <param name="cy">중심 위치 (cy) (mm)</param>
        /// <param name="cz">중심 위치 (cz) (mm)</param>
        /// <param name="sweepAngle">회전 각도 (+ : 반시계방향)</param>
        /// <returns></returns>
        public bool ListArc(float cx, float cy, float cz, float sweepAngle) => this.ListArc(new Vector3(cx, cy, cz), sweepAngle);

        protected bool IsDuplicated(Vector3 vPosition)
        {
            Matrix4x4 matrix = new Matrix4x4(this.MatrixStack.ToResult);
            Vector3 vector3 = Vector3.Transform(vPosition, matrix);
            return MathHelper.IsEqual(this.vPhysical3D.X, vector3.X) && MathHelper.IsEqual(this.vPhysical3D.Y, vector3.Y) && MathHelper.IsEqual(this.vPhysical3D.Z, vector3.Z);
        }

        /// <summary>입력 엔코더의 초기화 (오프셋값 Dx, Dy를 설정 가능)</summary>
        /// <param name="offsetX">X 초기화 위치 (mm)</param>
        /// <param name="offsetY">Y 초기화 위치 (mm)</param>
        /// <returns></returns>
        public bool CtlEncoderReset(float offsetX = 0.0f, float offsetY = 0.0f)
        {
            RTC5Wrap.n_init_fly_2d(this.Index + 1U, (int)((double)this.EncXCountsPerMm * (double)offsetX), (int)((double)this.EncYCountsPerMm * (double)offsetY));
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: motf encoder reset with offset {1:F3}, {2:F3}", (object)this.Index, (object)offsetX, (object)offsetY), Array.Empty<object>());
            return true;
        }

        /// <summary>외부 엔코더 입력 대신 내부 가상 엔코더를 활성화 및 가상 입력 엔코더 속도 지정</summary>
        /// <param name="encXSimulatedSpeed">RTC 내부 가상 엔코더X 속도 (mm/s)</param>
        /// <param name="encYSimulatedSpeed">RTC 내부 가상 엔코더Y 속도 (mm/s)</param>
        /// <returns></returns>
        public bool CtlEncoderSpeed(float encXSimulatedSpeed, float encYSimulatedSpeed)
        {
            if (0.0 == (double)encXSimulatedSpeed && 0.0 == (double)encYSimulatedSpeed)
                RTC5Wrap.n_simulate_encoder(this.Index + 1U, 0U);
            else if (0.0 != (double)encXSimulatedSpeed)
                RTC5Wrap.n_simulate_encoder(this.Index + 1U, 1U);
            else if (0.0 != (double)encYSimulatedSpeed)
                RTC5Wrap.n_simulate_encoder(this.Index + 1U, 2U);
            this.EncXSimulatedSpeed = encXSimulatedSpeed;
            this.EncYSimulatedSpeed = encYSimulatedSpeed;
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: motf encoder simulated with {1:F3}, {2:F3} mm/s", (object)this.Index, (object)encXSimulatedSpeed, (object)encYSimulatedSpeed), Array.Empty<object>());
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
            RTC5Wrap.n_get_encoder(this.Index + 1U, out encX, out encY);
            encXmm = encYmm = 0.0f;
            if (this.EncXCountsPerMm != 0)
                encXmm = (float)encX / (float)this.EncXCountsPerMm;
            if (this.EncYCountsPerMm != 0)
                encYmm = (float)encY / (float)this.EncYCountsPerMm;
            return true;
        }

        /// <summary>
        /// 트래킹 에러 보상
        /// (추천 : 스캔 헤드의 메뉴얼에 명기된 Tracking Error 시간을 지정)
        /// </summary>
        /// <param name="xUsec">X 축 보상 시간(usec)</param>
        /// <param name="yUsec">Y축 보상 시간(usec)</param>
        /// <returns></returns>
        public bool CtlTrackingError(uint xUsec, uint yUsec)
        {
            RTC5Wrap.n_set_fly_tracking_error(this.Index + 1U, xUsec / 10U, yUsec / 10U);
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: motf set tracking error xy= {1}, {2} usec", (object)this.Index, (object)xUsec, (object)yUsec), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 엔코더 테이블 보정 파일 로드
        /// 보정 테이블0 번의 포맷 예 :
        /// [Fly2DTable0]
        /// Encoder0 Encoder1 Encoder0_Delta Encoder1_Delta ;주석
        /// ...
        /// 추신) 모든 위치값은 bits 이므로 mm * kFactor 하여 bits 값을 구할것.
        /// 추신) 모든 bits 값은 +-524288 을 초과하지 말것
        /// </summary>
        /// <param name="fileName">보정 파일 이름 (경로포함), null 지정시 보정 리셋됨</param>
        /// <param name="tableNo">테이블 번호</param>
        /// <returns></returns>
        public bool CtlMotfCompensateTable(string fileName, uint tableNo = 0)
        {
            int num = RTC5Wrap.n_load_fly_2d_table(this.Index + 1U, fileName, tableNo);
            bool flag = false;
            if (num < 0)
            {
                Logger.Log(Logger.Type.Warn, string.Format("rtc5 [{0}]: motf encoder compensate table loaded: {1} with {2} points", (object)this.Index, (object)fileName, (object)Math.Abs(num)), Array.Empty<object>());
                flag = true;
            }
            switch (num)
            {
                case 0:
                    Logger.Log(Logger.Type.Warn, string.Format("rtc5 [{0}]: motf encoder compensate table reset", (object)this.Index), Array.Empty<object>());
                    flag = true;
                    break;
                case 1:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: motf encoder compensate table : no valid data points found", (object)this.Index), Array.Empty<object>());
                    break;
                case 2:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: motf encoder compensate table : out of memory", (object)this.Index), Array.Empty<object>());
                    break;
                case 3:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: motf encoder compensate table : file not found", (object)this.Index), Array.Empty<object>());
                    break;
                case 4:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: motf encoder compensate table : DSP memory error", (object)this.Index), Array.Empty<object>());
                    break;
                case 5:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: motf encoder compensate table : BUSY error", (object)this.Index), Array.Empty<object>());
                    break;
                case 8:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: motf encoder compensate table : RTC5 board is locked by another application", (object)this.Index), Array.Empty<object>());
                    break;
                case 13:
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: motf encoder compensate table : The specified table number was not found in the file.", (object)this.Index), Array.Empty<object>());
                    break;
            }
            return flag;
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
                    RTC5Wrap.n_set_ext_start_delay(this.Index + 1U, (int)((double)this.EncXCountsPerMm * (double)distance), 0U);
                    break;
                case RtcEncoder.EncY:
                    RTC5Wrap.n_set_ext_start_delay(this.Index + 1U, (int)((double)this.EncYCountsPerMm * (double)distance), 1U);
                    break;
            }
            Logger.Log(Logger.Type.Info, string.Format("rtc5 [{0}]: motf external control delay distance= {1}, {2:F3} mm", (object)this.Index, (object)enc, (object)distance), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 리스트 명령 - MOTF 리스트 명령 시작
        /// 엔코더 값 초기화시에는 CtlEncoderReset에서 설정한 오프셋 값으로 초기화되며,
        /// 초기화를 하지 않더라도 ListBegin 시에는 외부 트리거 (/START)를 사용가능하도록 설정하기 때문에
        /// 해당 트리거 신호가 활성화(Closed)되면 엔코더가 자동으로 리셋(초기화) 되도록 설정됨
        /// </summary>
        /// <param name="encoderReset">엔코더 X,Y 초기화 여부 (</param>
        /// <returns></returns>
        public bool ListMOTFBegin(bool encoderReset = false)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            double ScaleX = 0.0;
            double ScaleY = 0.0;
            if ((double)this.EncXSimulatedSpeed == 0.0 && (double)this.EncYSimulatedSpeed == 0.0)
            {
                if (this.EncXCountsPerMm != 0)
                    ScaleX = (double)this.KFactor / (double)this.EncXCountsPerMm;
                if (this.EncYCountsPerMm != 0)
                    ScaleY = (double)this.KFactor / (double)this.EncYCountsPerMm;
            }
            else if ((double)this.EncXSimulatedSpeed == 0.0)
            {
                if (this.EncXCountsPerMm == 0)
                {
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: motf invalid encoder0 counts/mm value)", (object)this.Index), Array.Empty<object>());
                    return false;
                }
                ScaleX = (double)this.KFactor / (double)this.EncXCountsPerMm;
                ScaleY = (double)this.KFactor * (double)this.EncYSimulatedSpeed / 1000000.0;
            }
            else if ((double)this.EncYSimulatedSpeed == 0.0)
            {
                if (this.EncYCountsPerMm == 0)
                {
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: motf invalid encoder1 counts/mm value)", (object)this.Index), Array.Empty<object>());
                    return false;
                }
                ScaleX = (double)this.KFactor * (double)this.EncXSimulatedSpeed / 1000000.0;
                ScaleY = (double)this.KFactor / (double)this.EncYCountsPerMm;
            }
            else
            {
                if (0.0 == (double)this.EncXSimulatedSpeed || 0.0 == (double)this.EncYSimulatedSpeed)
                {
                    Logger.Log(Logger.Type.Error, string.Format("rtc5 [{0}]: motf invalid encoder0,1 counts/mm value)", (object)this.Index), Array.Empty<object>());
                    return false;
                }
                ScaleX = (double)this.KFactor * (double)this.EncXSimulatedSpeed / 1000000.0;
                ScaleY = (double)this.KFactor * (double)this.EncYSimulatedSpeed / 1000000.0;
            }
            if (0.0 == ScaleX)
                ScaleX = ScaleY;
            if (0.0 == ScaleY)
                ScaleY = ScaleX;
            if (!this.IsListReady(1U))
                return false;
            if (encoderReset)
                RTC5Wrap.n_set_fly_2d(this.Index + 1U, ScaleX, ScaleY);
            else
                RTC5Wrap.n_activate_fly_2d(this.Index + 1U, ScaleX, ScaleY);
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
                    RTC5Wrap.n_set_ext_start_delay_list(this.Index + 1U, (int)((double)this.EncXCountsPerMm * (double)distance), 0U);
                    break;
                case RtcEncoder.EncY:
                    RTC5Wrap.n_set_ext_start_delay_list(this.Index + 1U, (int)((double)this.EncYCountsPerMm * (double)distance), 1U);
                    break;
            }
            return true;
        }

        /// <summary>
        /// 리스트 명령 - 지정된 엔코더 단축(X 혹은 Y)의 위치가 특정 조건을 만족할때까지 리스트 명령 대기
        /// (단축 동기화 용)
        /// </summary>
        /// <param name="enc">엔코더 축 지정</param>
        /// <param name="position">위치값 (mm)</param>
        /// <param name="cond">대기 조건</param>
        /// <returns></returns>
        public bool ListMOTFWait(RtcEncoder enc, float position, EncoderWaitCondition cond)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            switch (enc)
            {
                case RtcEncoder.EncX:
                    RTC5Wrap.n_wait_for_encoder_mode(this.Index + 1U, (int)((double)position * (double)this.EncXCountsPerMm), 0U, (int)cond);
                    break;
                case RtcEncoder.EncY:
                    RTC5Wrap.n_wait_for_encoder_mode(this.Index + 1U, (int)((double)position * (double)this.EncXCountsPerMm), 1U, (int)cond);
                    break;
            }
            return true;
        }

        /// <summary>리스트 명령 - 두개의 엔코더가 (X, Y)가 특정 조건이 될때 까지 대기 (다축 동기화 용)</summary>
        /// <param name="positionX">X 축 위치 (mm)</param>
        /// <param name="rangeX">조건 범위 (mm)</param>
        /// <param name="positionY">Y 축 위치 (mm)</param>
        /// <param name="rangeY">조건 범위(mm)</param>
        public bool ListMOTFWaits(float positionX, float rangeX, float positionY, float rangeY)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            int EncXmin = (int)((double)this.EncXCountsPerMm * ((double)positionX - (double)rangeX / 2.0));
            int EncXmax = (int)((double)this.EncXCountsPerMm * ((double)positionX + (double)rangeX / 2.0));
            int encXcountsPerMm = this.EncXCountsPerMm;
            int EncYmin = (int)((double)this.EncYCountsPerMm * ((double)positionY - (double)rangeY / 2.0));
            int EncYmax = (int)((double)this.EncYCountsPerMm * ((double)positionY + (double)rangeY / 2.0));
            int encYcountsPerMm = this.EncYCountsPerMm;
            if (EncXmin > EncXmax)
            {
                int num = EncXmax;
                EncXmax = EncXmin;
                EncXmin = num;
            }
            if (EncYmin > EncYmax)
            {
                int num = EncYmax;
                EncYmax = EncYmin;
                EncYmin = num;
            }
            if (!this.IsListReady(1U))
                return false;
            RTC5Wrap.n_wait_for_encoder_in_range(this.Index + 1U, EncXmin, EncXmax, EncYmin, EncYmax);
            return true;
        }

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
            int X = (int)((double)vector2.X * (double)this.KFactor);
            int Y = (int)((double)vector2.Y * (double)this.KFactor);
            if (!this.IsListReady(1U))
                return false;
            RTC5Wrap.n_fly_return(this.Index + 1U, X, Y);
            this.vPhysical = vector2;
            this.vLogical = vPosition;
            return true;
        }

        /// <summary>
        /// 특정 색인 문자 좌표 정보 저장 시작
        /// 이 명령 이후 해당 문자(character)에 대한 리스트 명령 (jump, mark, arc) 명령 호출 필요
        /// </summary>
        /// <param name="asciiCode">아스키 코드 (0~255)</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        public bool CtlCharacterBegin(uint asciiCode, CharacterSet characterSet = CharacterSet._0)
        {
            uint Char = (uint)characterSet * 256U + asciiCode;
            if (Char > 1023U)
            {
                Logger.Log(Logger.Type.Error, string.Format("rtc5 invalid character set= {0}. ascii code= {1}", (object)characterSet, (object)asciiCode), Array.Empty<object>());
                return false;
            }
            RTC5Wrap.n_load_char(this.Index + 1U, Char);
            return true;
        }

        /// <summary>
        /// 보호된 리스트 버퍼(3) 영역에 색인 문자 저장 완료
        /// CtlCharacterSetBegin 함수와 짝이 되어 문자 리스트 명령 기록 완료시 호출
        /// </summary>
        /// <returns></returns>
        public bool CtlCharacterEnd()
        {
            RTC5Wrap.n_list_return(this.Index + 1U);
            return true;
        }

        /// <summary>보호된 리스트 버퍼(3) 영역에 색인된 문자가 있는지 여부</summary>
        /// <param name="asciiCode">아스키 코드 (0~255)</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        public bool CtlCharacterSetIsExist(uint asciiCode, CharacterSet characterSet = CharacterSet._0)
        {
            uint Char = (uint)characterSet * 256U + asciiCode;
            if (Char <= 1023U)
                return uint.MaxValue != RTC5Wrap.n_get_char_pointer(this.Index + 1U, Char);
            Logger.Log(Logger.Type.Error, string.Format("rtc5 invalid character set= {0}. ascii code= {1}", (object)characterSet, (object)asciiCode), Array.Empty<object>());
            return false;
        }

        /// <summary>지정된 색인 문자열 집합을 삭제합니다</summary>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        public bool CtlCharacterSetClear(CharacterSet characterSet)
        {
            for (uint index = 0; index < (uint)byte.MaxValue; ++index)
            {
                uint Char = (uint)characterSet * 256U + index;
                RTC5Wrap.n_load_char(this.Index + 1U, Char);
                RTC5Wrap.n_list_return(this.Index + 1U);
            }
            return true;
        }

        /// <summary>시리얼 번호 리셋</summary>
        /// <param name="serialNo">시작 번호</param>
        /// <param name="incrementStep">증가 값</param>
        /// <returns></returns>
        public bool CtlSerialReset(uint serialNo, uint incrementStep = 1)
        {
            RTC5Wrap.n_set_serial_step(this.Index + 1U, serialNo, incrementStep);
            this.SerialStartNo = serialNo;
            this.SerialIncrementStep = incrementStep;
            Logger.Log(Logger.Type.Warn, string.Format("rtc5 [{0}]: reset to serial {1}, {2}", (object)this.Index, (object)serialNo, (object)incrementStep), Array.Empty<object>());
            return true;
        }

        /// <summary>보호된 리스트 버퍼(3) 영역에 색인된 문자를 이용해 문자열 마킹</summary>
        /// <param name="text">문자열</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        public bool ListText(string text, CharacterSet characterSet = CharacterSet._0)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(2U))
                return false;
            RTC5Wrap.n_select_char_set(this.Index + 1U, (uint)characterSet);
            RTC5Wrap.n_mark_text_abs(this.Index + 1U, text);
            this.vPhysical = new Vector2(float.MaxValue, float.MaxValue);
            this.vLogical = new Vector2(float.MaxValue, float.MaxValue);
            this.stream?.WriteLine("TEXT = " + text);
            return true;
        }

        /// <summary>보호된 리스트 버퍼(3) 영역에 색인된 문자를 이용해 날짜 마킹</summary>
        /// <param name="dateFormat">DateFormat 열거형</param>
        /// <param name="leadingWithZero">앞선 공간을 0 으로 채우기</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        public bool ListDate(DateFormat dateFormat, bool leadingWithZero, CharacterSet characterSet = CharacterSet._0)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(3U))
                return false;
            RTC5Wrap.n_time_fix(this.Index + 1U);
            RTC5Wrap.n_select_char_set(this.Index + 1U, (uint)characterSet);
            uint Mode = (uint)(0 | 2);
            if (leadingWithZero)
                Mode |= 1U;
            RTC5Wrap.n_mark_date_abs(this.Index + 1U, (uint)dateFormat, Mode);
            this.vPhysical = new Vector2(float.MaxValue, float.MaxValue);
            this.vLogical = new Vector2(float.MaxValue, float.MaxValue);
            this.stream?.WriteLine("DATE= " + dateFormat.ToString());
            return true;
        }

        /// <summary>
        /// 보호된 리스트 버퍼(3) 영역에 색인된 문자를 이용해 시간 마킹
        /// 호출 시점의 윈도우즈 시스템 시간을 사용함
        /// </summary>
        /// <param name="timeFormat">TimeFormat 열거형</param>
        /// <param name="leadingWithZero">앞선 공간을 0 으로 채우기</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        public bool ListTime(TimeFormat timeFormat, bool leadingWithZero, CharacterSet characterSet = CharacterSet._0)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(3U))
                return false;
            RTC5Wrap.n_time_fix(this.Index + 1U);
            RTC5Wrap.n_select_char_set(this.Index + 1U, (uint)characterSet);
            uint Mode = (uint)(0 | 2);
            if (leadingWithZero)
                Mode |= 1U;
            RTC5Wrap.n_mark_time_abs(this.Index + 1U, (uint)timeFormat, Mode);
            this.vPhysical = new Vector2(float.MaxValue, float.MaxValue);
            this.vLogical = new Vector2(float.MaxValue, float.MaxValue);
            this.stream?.WriteLine("TIME= " + timeFormat.ToString());
            return true;
        }

        /// <summary>보호된 리스트 버퍼(3) 영역에 색인된 문자를 이용해 시리얼 번호 마킹</summary>
        /// <param name="numOfDigits">최대 자리수 (최대 15자)</param>
        /// <param name="serialFormat">SerialFormat 열거형</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        public bool ListSerial(uint numOfDigits, SerialFormat serialFormat, CharacterSet characterSet = CharacterSet._0)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if (numOfDigits > 12U)
            {
                Logger.Log(Logger.Type.Error, string.Format("rtc5 overflow serial number of digits= {0}", (object)numOfDigits), Array.Empty<object>());
                return false;
            }
            if (!this.IsListReady(2U))
                return false;
            RTC5Wrap.n_select_char_set(this.Index + 1U, (uint)characterSet);
            uint Mode = (uint)(0 + 20 + serialFormat);
            RTC5Wrap.n_mark_serial_abs(this.Index + 1U, Mode, numOfDigits);
            this.vPhysical = new Vector2(float.MaxValue, float.MaxValue);
            this.vLogical = new Vector2(float.MaxValue, float.MaxValue);
            this.stream?.WriteLine("SERIAL= " + serialFormat.ToString());
            return true;
        }
    }
}
