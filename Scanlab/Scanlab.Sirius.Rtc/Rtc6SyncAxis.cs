// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Rtc6SyncAxis

using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading;

namespace Scanlab.Sirius
{
    /// <summary>
    /// SCANLAB's XLSCAN (RTC6 + SyncAXIS + ExcelliSCAN + ACS Motion)
    /// x64 버전만 지원 가능
    /// </summary>
    public class Rtc6SyncAxis : IRtc, IDisposable, IRtcSyncAxis
    {
        public static int Count;
        private string[] ctbFileName = new string[4];
        private string configXMLFile;
        protected uint listTotalCount;
        private bool aborted;
        private Vector2 vPhysical;
        private Vector2 vLogical;
        private string outputFileName;
        private StreamWriter stream;
        private ILaser laser;
        internal uint handle;
        internal JobStatus jobStatus = new JobStatus();
        private slsc_OperationMode oldOpMode;
        //private Rockey4ND rockey = new Rockey4ND();
        protected bool isManualOn;
        private bool disposed;

        /// <summary>RTC 제어기 식별 번호 (1,2,3,...)</summary>
        public uint Index { get; set; }

        /// <summary>이름</summary>
        public string Name { get; set; }

        /// <summary>행렬 스택</summary>
        public IMatrixStack MatrixStack { get; set; }

        /// <summary>bits/mm 값</summary>
        public float KFactor { get; set; }

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
        public CorrectionTableIndex PrimaryHeadTable { get; private set; }

        /// <summary>XML 환경 설정 파일</summary>
        public string ConfigXMLFile
        {
            get => this.configXMLFile;
            set
            {
                syncAXIS.slsc_cfg_delete(this.handle);
                this.handle = 0U;
                if (syncAXIS.slsc_cfg_initialize_from_file(out this.handle, value) != 0U)
                    return;
                this.ReInitialize();
                this.configXMLFile = value;
            }
        }

        /// <summary>스테이지 이동 속도 (mm/s)</summary>
        public float StageMoveSpeed { get; set; }

        /// <summary>스테이지 이동시 타임아웃 시간 (s)</summary>
        public float StageMoveTimeOut { get; set; }

        /// <summary>LPF(Low Pass Filter) Bandwidth</summary>
        public float BandWidth { get; private set; }

        /// <summary>생성자</summary>
        public Rtc6SyncAxis()
        {
            this.MatrixStack = (IMatrixStack)new Scanlab.Sirius.MatrixStack();
            this.vPhysical = Vector2.Zero;
            this.vLogical = Vector2.Zero;
            this.listTotalCount = 0U;
            this.stream = (StreamWriter)null;
            this.handle = 0U;
            this.StageMoveSpeed = 10f;
            this.StageMoveTimeOut = 5f;
            ++Rtc6SyncAxis.Count;
        }

        /// <summary>생성자</summary>
        /// <param name="index">식별번호 (1,2,3, ...)</param>
        /// <param name="configXMLFile">SyncAxis 환경설정(xml) 파일</param>
        /// <param name="outputFileName">RTC 데이타 상세 로그 출력 파일</param>
        public Rtc6SyncAxis(uint index, string configXMLFile, string outputFileName = "")
          : this()
        {
            this.Index = index;
            this.outputFileName = outputFileName;
            this.configXMLFile = configXMLFile;
        }

        /// <summary>종결자</summary>
        ~Rtc6SyncAxis()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        public void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
                --Rtc6SyncAxis.Count;
                int count = Rtc6SyncAxis.Count;
            }
            this.disposed = true;
        }

        /// <summary>초기화 : 리스트 핸들링 모드가 RepeatWhileBufferFull 로 자동 설정됨</summary>
        /// <param name="unUsed_kFactor"></param>
        /// <param name="unUsed_laserMode"></param>
        /// <param name="unUsed_ctbFileName"></param>
        /// <returns></returns>
        public bool Initialize(
          float unUsed_kFactor,
          LaserMode unUsed_laserMode,
          string unUsed_ctbFileName)
        {
            //if (!this.rockey.Initialize() || !this.rockey.IsRtcSyncLicensed)
            //    this.rockey.InvalidLicense();
            this.KFactor = unUsed_kFactor;
            uint errorCode = syncAXIS.slsc_cfg_initialize_from_file(out this.handle, this.ConfigXMLFile);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to initialize {1} : error = {2} detail= {3}", (object)this.Index, (object)this.ConfigXMLFile, (object)this.CtlGetErrMsg(errorCode), (object)this.CtlGetInternalErrMsg()), Array.Empty<object>());
                return false;
            }
            VersionInfo syncAxisVersion = syncAXIS.slsc_cfg_get_sync_axis_version();
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: handle id : {1}. config : {2}. version : {3}, {4}, {5}", (object)this.Index, (object)this.handle, (object)this.ConfigXMLFile, (object)syncAxisVersion.Major, (object)syncAxisVersion.Minor, (object)syncAxisVersion.Revision), Array.Empty<object>());
            this.IsScanAhead = this.IsSyncAxis = true;
            this.LaserMode = LaserMode.None;
            this.Fpk = 0.0f;
            return this.ReInitialize();
        }

        protected bool ReInitialize()
        {
            syncAXIS.slsc_cfg_get_trajectory_config(this.handle);
            int num1 = (int)syncAXIS.slsc_cfg_register_callback_job_start_planned(this.handle, (JobCallback)new CallBackJobStartCalculationPlanned(this));
            int num2 = (int)syncAXIS.slsc_cfg_register_callback_job_progress_planned(this.handle, (ExecTimeCallback)new CallBackJobProgressCalculationPlanned(this));
            int num3 = (int)syncAXIS.slsc_cfg_register_callback_job_end_planned(this.handle, (ExecTimeCallback)new CallBackJobEndCalculationPlanned(this));
            int num4 = (int)syncAXIS.slsc_cfg_register_callback_job_loaded_enough(this.handle, (JobCallback)new CallBackJobLoadedEnoughTransfer(this));
            int num5 = (int)syncAXIS.slsc_cfg_register_callback_job_is_executing(this.handle, (ExecTimeCallback)new CallBackJobIsExecuting(this));
            int num6 = (int)syncAXIS.slsc_cfg_register_callback_job_finished_executing(this.handle, (ExecTimeCallback)new CallBackJobFinishedExecuting(this));
            int num7 = (int)syncAXIS.slsc_cfg_set_list_handling_mode_with_context(this.handle, slsc_ListHandlingMode.slsc_ListHandlingMode_RepeatWhileBufferFull, (ListHandlingCallback)new CallBackListHandling(this));
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: reset listhandling mode to {1}", (object)this.Index, (object)slsc_ListHandlingMode.slsc_ListHandlingMode_RepeatWhileBufferFull), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 보정 파일 변경 - syncAxis 미지원
        /// (XML 설정 파일에 등록된 ct5 파일이 대신 사용됨)
        /// </summary>
        /// <param name="tableIndex"></param>
        /// <param name="ctbFileName"></param>
        /// <returns></returns>
        public bool CtlLoadCorrectionFile(CorrectionTableIndex tableIndex, string ctbFileName)
        {
            if (this.handle == 0U || this.CtlGetStatus(RtcStatus.Busy))
                return false;
            Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: load correction file is not supported yet !", (object)this.Index), Array.Empty<object>());
            return false;
        }

        /// <summary>
        /// 보정 파일 선택 - syncAxis 미지원
        /// (XML 설정 파일에 등록된 ct5 파일에서 선택됨)
        /// </summary>
        /// <param name="primaryHeadTableIndex"></param>
        /// <param name="secondaryHeadTableIndex"></param>
        /// <returns></returns>
        public bool CtlSelectCorrection(
          CorrectionTableIndex primaryHeadTableIndex,
          CorrectionTableIndex secondaryHeadTableIndex = CorrectionTableIndex.None)
        {
            if (this.handle == 0U)
                return false;
            uint num = syncAXIS.slsc_ctrl_select_correction_file(this.handle, (uint)(primaryHeadTableIndex - 1));
            if (num == 0U)
            {
                this.PrimaryHeadTable = primaryHeadTableIndex;
                Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: correction file selected : primary head= {1}", (object)this.Index, (object)primaryHeadTableIndex.ToString()), Array.Empty<object>());
            }
            else
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to select correction file : primary head= {1}", (object)this.Index, (object)primaryHeadTableIndex.ToString()), Array.Empty<object>());
            return num == 0U;
        }

        /// <summary>현재 설정된 주파수, 펄스폭 으로 레이저 변조 신호(LASER1,2,ON) 출력</summary>
        /// <returns></returns>
        public bool CtlLaserOn()
        {
            if (this.handle == 0U)
                return false;
            uint errorCode = 0U | syncAXIS.slsc_ctrl_unfollow(this.handle) | syncAXIS.slsc_ctrl_laser_signal_on(this.handle);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_ctrl_laser_signal_on : error {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
            this.isManualOn = true;
            Logger.Log(Logger.Type.Warn, string.Format("syncaxis [{0}]: laser signal on !", (object)this.Index), Array.Empty<object>());
            return true;
        }

        /// <summary>레이저 변호 신호 (LASER1,2,ON) 중단</summary>
        /// <returns></returns>
        public bool CtlLaserOff()
        {
            if (this.handle == 0U)
                return false;
            uint errorCode = 0U | syncAXIS.slsc_ctrl_laser_signal_off(this.handle) | syncAXIS.slsc_ctrl_follow(this.handle);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_ctrl_laser_signal_off : error {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
            this.isManualOn = false;
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: laser signal off !", (object)this.Index), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 미사용 할것 / ISyncAxis 에서 별도의 CtlMove 제공됨
        /// (스캐너 위치 이동)
        /// </summary>
        /// <param name="vPosition"></param>
        /// <returns></returns>
        public bool CtlMove(Vector2 vPosition)
        {
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: please call another ctlmove function to move scanner or stage", (object)this.Index), Array.Empty<object>());
            return false;
        }

        /// <summary>
        /// 미사용 할것 / ISyncAxis 에서 별도의 CtlMove 제공됨
        /// (스캐너 위치 이동)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool CtlMove(float x, float y) => this.CtlMove(new Vector2(x, y));

        /// <summary>주파수와 펄스폭 설정</summary>
        /// <param name="frequency">주파수 (Hz)</param>
        /// <param name="pulseWidth">펄스폭 (usec)</param>
        /// <returns></returns>
        public bool CtlFrequency(float frequency, float pulseWidth)
        {
            uint num = syncAXIS.slsc_ctrl_set_laser_pulses(this.handle, 1.0 / (double)frequency / 2.0, (double)pulseWidth / 1000000.0);
            if (num == 0U)
                Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: frequency {1:F3}Hz. pulse width {2:F3}usec", (object)this.Index, (object)frequency, (object)pulseWidth), Array.Empty<object>());
            else
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to frequency {1:F3}Hz. pulse width {2:F3}usec", (object)this.Index, (object)frequency, (object)pulseWidth), Array.Empty<object>());
            return num == 0U;
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
            Logger.Log(Logger.Type.Warn, string.Format("syncaxis [{0}]: set laser/scanner delay is not allowed", (object)this.Index), Array.Empty<object>());
            return true;
        }

        /// <summary>스캐너 점프/마크 속도 설정</summary>
        /// <param name="jump">점프(jump) 속도 (mm/s)</param>
        /// <param name="mark">마크(mark) 및 아크(arc) 속도 (mm/s)</param>
        /// <returns></returns>
        public bool CtlSpeed(float jump, float mark) => !this.CtlGetStatus(RtcStatus.Busy) && (0U | syncAXIS.slsc_cfg_set_jump_speed(this.handle, (double)jump) | syncAXIS.slsc_cfg_set_mark_speed(this.handle, (double)mark)) == 0U;

        /// <summary>확장 포트에 데이타 쓰기</summary>
        /// <typeparam name="T">값(16비트, 8비트, 2비트 (uint), 아나로그(float 10V)</typeparam>
        /// <param name="ch">확장 커넥터 종류 </param>
        /// <param name="value">uint/float</param>
        /// <param name="compensator">compensator 보정용 객체</param>
        /// <returns></returns>
        public bool CtlWriteData<T>(ExtensionChannel ch, T value, ICompensator<T> compensator = null)
        {
            uint num1 = 0;
            switch (ch)
            {
                case ExtensionChannel.ExtDO2:
                case ExtensionChannel.ExtDO8:
                    return false;
                case ExtensionChannel.ExtDO16:
                    num1 = syncAXIS.slsc_ctrl_write_digital_out(this.handle, (ushort)Convert.ChangeType((object)value, typeof(ushort)));
                    break;
                case ExtensionChannel.ExtAO1:
                    float num2 = (float)Convert.ChangeType((object)value, typeof(float));
                    num1 = syncAXIS.slsc_ctrl_write_analog_x(this.handle, slsc_AnalogOutput.slsc_AnalogOutput_1, (Math.Pow(2.0, 12.0) - 1.0) * (double)num2 / 10.0);
                    break;
                case ExtensionChannel.ExtAO2:
                    float num3 = (float)Convert.ChangeType((object)value, typeof(float));
                    num1 = syncAXIS.slsc_ctrl_write_analog_x(this.handle, slsc_AnalogOutput.slsc_AnalogOutput_2, (Math.Pow(2.0, 12.0) - 1.0) * (double)num3 / 10.0);
                    break;
            }
            return num1 == 0U;
        }

        /// <summary>확장1 포트의 16비트 디지털 출력의 특정 비트값을 변경</summary>
        /// <param name="bitPosition">0~15</param>
        /// <param name="onOff">출력</param>
        /// <returns></returns>
        public bool CtlWriteExtDO16(ushort bitPosition, bool onOff)
        {
            ushort Mask = (ushort)(1U << (int)bitPosition);
            int num = (int)syncAXIS.slsc_ctrl_write_digital_out_mask(this.handle, onOff ? Mask : (ushort)0, Mask);
            this.stream?.WriteLine(string.Format("DATA : ExtDO16 : BIT{0} : {1}", (object)bitPosition, (object)onOff.ToString()));
            return true;
        }

        /// <summary>확장 포트에서 데이타 읽기 - 미지원</summary>
        /// <typeparam name="T">값(16비트, 8비트, 2비트 (uint), 아나로그(float 10V)</typeparam>
        /// <param name="ch"></param>
        /// <param name="value">uint/float</param>
        /// <param name="compensator">compensator 보정용 객체</param>
        /// <returns></returns>
        public bool CtlReadData<T>(ExtensionChannel ch, out T value, ICompensator<T> compensator = null)
        {
            value = default(T);
            return false;
        }

        /// <summary>SYNCAXIS 의 에러코드 조회</summary>
        /// <param name="errorCode">에러코드</param>
        /// <returns></returns>
        public string CtlGetErrMsg(uint errorCode) => new SyncAxisError((int)errorCode).ToString();

        /// <summary>RTC 카드의 상태 확인</summary>
        /// <param name="status">RtcStatus 열거형</param>
        /// <returns></returns>
        public bool CtlGetStatus(RtcStatus status)
        {
            bool flag1 = false;
            switch (status)
            {
                case RtcStatus.Busy:
                    if (this.handle == 0U)
                    {
                        flag1 = false;
                        break;
                    }
                    slsc_ExecState State = slsc_ExecState.slsc_ExecState_Idle;
                    int execState = (int)syncAXIS.slsc_ctrl_get_exec_state(this.handle, out State);
                    if (slsc_ExecState.slsc_ExecState_Executing == State)
                    {
                        flag1 = true;
                        break;
                    }
                    break;
                case RtcStatus.NotBusy:
                    flag1 = !this.CtlGetStatus(RtcStatus.Busy);
                    break;
                case RtcStatus.List1Busy:
                    flag1 = this.CtlGetStatus(RtcStatus.Busy);
                    break;
                case RtcStatus.List2Busy:
                    flag1 = this.CtlGetStatus(RtcStatus.Busy);
                    break;
                case RtcStatus.NoError:
                    if (this.handle == 0U)
                    {
                        flag1 = false;
                        break;
                    }
                    uint ErrorCount;
                    bool flag2 = syncAXIS.slsc_ctrl_get_error_count(this.handle, out ErrorCount) != 0U || ErrorCount > 0U;
                    flag1 = !this.CtlGetStatus(RtcStatus.Aborted) && !flag2;
                    break;
                case RtcStatus.Aborted:
                    flag1 = this.aborted;
                    break;
                case RtcStatus.PositionAckOK:
                    flag1 = true;
                    break;
                case RtcStatus.PowerOK:
                    flag1 = true;
                    break;
                case RtcStatus.TempOK:
                    flag1 = true;
                    break;
            }
            return flag1;
        }

        /// <summary>리스트 명령이 완료될 때(busy 가 해제될때) 까지 대기하는 함수</summary>
        /// <returns></returns>
        public bool CtlBusyWait()
        {
            if (this.handle == 0U)
                return false;
            slsc_ExecState State = slsc_ExecState.slsc_ExecState_Idle;
            while (!this.aborted && syncAXIS.slsc_ctrl_get_exec_state(this.handle, out State) == 0U)
            {
                Thread.Sleep(10);
                if (slsc_ExecState.slsc_ExecState_Executing != State)
                    return true;
            }
            return false;
        }

        /// <summary>실행중인 리스트 명령(busy 상태를)을 강제 종료</summary>
        /// <returns></returns>
        public bool CtlAbort()
        {
            if (this.handle == 0U)
                return false;
            if (!this.aborted)
            {
                uint errorCode = syncAXIS.slsc_ctrl_stop(this.handle);
                if (errorCode != 0U)
                    Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to abort : {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
            }
            this.aborted = true;
            this.stream?.Flush();
            this.stream?.Dispose();
            this.stream = (StreamWriter)null;
            Logger.Log(Logger.Type.Warn, string.Format("syncaxis [{0}]: aborted ! reset for re-initialize", (object)this.Index), Array.Empty<object>());
            return true;
        }

        /// <summary>에러상태를 해제</summary>
        /// <returns></returns>
        public bool CtlReset()
        {
            if (!this.aborted)
                return true;
            uint errorCode = syncAXIS.slsc_cfg_reinitialize_from_file(this.handle, this.ConfigXMLFile);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_cfg_reinitialize_from_file {1} : error = {2} detail= {3}", (object)this.Index, (object)this.ConfigXMLFile, (object)this.CtlGetErrMsg(errorCode), (object)this.CtlGetInternalErrMsg()), Array.Empty<object>());
                return false;
            }
            this.ReInitialize();
            this.aborted = false;
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: reset for clear !", (object)this.Index), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 스캐너 혹은 스테이지 이동
        /// (스테이지 이동일 경우 StageMoveSpeed / StageMoveTimeOut 값이 사용됨)
        /// </summary>
        /// <param name="motionType">모션 타입</param>
        /// <param name="vPosition">X,Y 위치 (mm)</param>
        /// <returns></returns>
        public bool CtlMove(MotionType motionType, Vector2 vPosition)
        {
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            double[] Position = new double[2]
            {
        (double) vector2.X,
        (double) vector2.Y
            };
            uint num = 0U | syncAXIS.slsc_ctrl_unfollow(this.handle);
            switch (motionType)
            {
                case MotionType.ScannerOnly:
                    num |= syncAXIS.slsc_ctrl_move_scanner_abs(this.handle, Position);
                    break;
                case MotionType.StageOnly:
                    num |= syncAXIS.slsc_ctrl_move_stage_abs(this.handle, Position, (double)this.StageMoveSpeed, (double)this.StageMoveTimeOut);
                    break;
                case MotionType.StageAndScanner:
                    num = num | syncAXIS.slsc_ctrl_move_scanner_abs(this.handle, Position) | syncAXIS.slsc_ctrl_move_stage_abs(this.handle, Position, (double)this.StageMoveSpeed, (double)this.StageMoveTimeOut);
                    break;
            }
            uint errorCode = num | syncAXIS.slsc_ctrl_follow(this.handle);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to move : error {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]:{1} moved {2:F3}, {3:F3}", (object)this.Index, (object)motionType.ToString(), (object)vPosition.X, (object)vPosition.Y), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 스캐너 혹은 스테이지 이동
        /// (스테이지 이동일 경우 StageMoveSpeed / StageMoveTimeOut 값이 사용됨)
        /// </summary>
        /// <param name="motionType">모션 타입</param>
        /// <param name="x">X 위치 (mm)</param>
        /// <param name="y">Y 위치 (mm) </param>
        /// <returns></returns>
        public bool CtlMove(MotionType motionType, float x, float y) => this.CtlMove(motionType, new Vector2(x, y));

        /// <summary>LPF (Low Pass Filter)의 Cut Off 대역폭</summary>
        /// <param name="filterBandWidth"></param>
        /// <returns></returns>
        public bool CtlBandWidth(float filterBandWidth)
        {
            if (this.handle == 0U)
                return false;
            uint errorCode = 0U | syncAXIS.slsc_cfg_set_bandwidth(this.handle, (double)filterBandWidth);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_cfg_set_bandwidth : error {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
            this.BandWidth = filterBandWidth;
            Logger.Log(Logger.Type.Warn, string.Format("syncaxis [{0}]: bandwidth filter= {1:F3} Hz", (object)this.Index, (object)filterBandWidth), Array.Empty<object>());
            return true;
        }

        /// <summary>멀티 헤드 오프셋 적용</summary>
        /// <param name="scanDevice">멀티헤드번호</param>
        /// <param name="offset">오프셋 량</param>
        /// <param name="angle">회전량</param>
        /// <returns></returns>
        public bool CtlHeadOffset(ScanDevice scanDevice, Vector2 offset, float angle)
        {
            if (this.handle == 0U)
                return false;
            double[] Offset = new double[2]
            {
        (double) offset.X,
        (double) offset.Y
            };
            Matrix3x2 rotation = Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f));
            double[] Matrix = new double[4]
            {
        (double) rotation.M11,
        (double) rotation.M12,
        (double) rotation.M21,
        (double) rotation.M21
            };
            uint errorCode = 0U | syncAXIS.slsc_cfg_set_part_displacement(this.handle, (slsc_ScanDevice)scanDevice, Matrix, Offset);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_cfg_set_part_displacement : error {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
            Logger.Log(Logger.Type.Warn, string.Format("syncaxis [{0}]: head offset xy={1:F3}, {2:F3}, angle={3:F3} ", (object)this.Index, (object)offset.X, (object)offset.Y, (object)angle), Array.Empty<object>());
            return true;
        }

        internal string CtlGetInternalErrMsg()
        {
            uint ErrorCount;
            int errorCount = (int)syncAXIS.slsc_ctrl_get_error_count(this.handle, out ErrorCount);
            if (ErrorCount <= 0U)
                return string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            for (uint ErrorNr = 0; ErrorNr < ErrorCount; ++ErrorNr)
            {
                ulong ErrorCode;
                string ErrorMsg;
                int error = (int)syncAXIS.slsc_ctrl_get_error(this.handle, ErrorNr, out ErrorCode, out ErrorMsg);
                stringBuilder.Append(string.Format("{0} : ", (object)ErrorCode));
                stringBuilder.Append(ErrorMsg);
                if ((int)ErrorNr != (int)ErrorCount - 1)
                    stringBuilder.Append(", ");
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 리스트 명령 시작 - 버퍼 준비 (Scanner Only 방식으로 고정됨)
        /// * 전용 ListBegin 함수를 사용할것을 추천함
        /// </summary>
        /// <param name="laser">레이저 소스</param>
        /// <param name="listType">리스트 타입 (하나의 거대한 리스트 : single, 더블 버퍼링되는 두개의 리스트 : double)</param>
        /// <returns></returns>
        public bool ListBegin(ILaser laser, ListType listType = ListType.Single)
        {
            Logger.Log(Logger.Type.Warn, string.Format("syncaxis [{0}]: use another list begin function instead !", (object)this.Index), Array.Empty<object>());
            return this.ListBegin(laser, MotionType.ScannerOnly);
        }

        /// <summary>
        /// 리스트 명령 시작 - 자동으로 slsc_ListHandlingMode_RepeatWhileBufferFull 처리됨
        /// </summary>
        /// <param name="laser">레이저 소스</param>
        /// <param name="motionType">스캐너 단독, 스테이지 단독, 스캐너+스테이지 혼합</param>
        /// <returns></returns>
        public bool ListBegin(ILaser laser, MotionType motionType)
        {
            //if (!this.rockey.IsRtcLicensed)
            //    this.rockey.InvalidLicense();
            if (this.handle == 0U)
                return false;
            if (!string.IsNullOrEmpty(this.outputFileName))
            {
                this.stream?.Dispose();
                this.stream = new StreamWriter(this.outputFileName);
            }
            this.laser = laser;
            this.aborted = false;
            this.listTotalCount = 0U;
            uint num = 0;
            switch (motionType)
            {
                case MotionType.ScannerOnly:
                    uint errorCode1 = num | syncAXIS.slsc_cfg_get_mode(this.handle, out this.oldOpMode) | syncAXIS.slsc_cfg_set_mode(this.handle, slsc_OperationMode.slsc_OperationMode_ScannerOnly);
                    if (errorCode1 != 0U)
                    {
                        Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_cfg_get_mode : {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode1)), Array.Empty<object>());
                        return false;
                    }
                    break;
                case MotionType.StageOnly:
                    uint errorCode2 = num | syncAXIS.slsc_cfg_get_mode(this.handle, out this.oldOpMode) | syncAXIS.slsc_cfg_set_mode(this.handle, slsc_OperationMode.slsc_OperationMode_StageOnly);
                    if (errorCode2 != 0U)
                    {
                        Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]:fail to slsc_cfg_get_mode : {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode2)), Array.Empty<object>());
                        return false;
                    }
                    break;
                case MotionType.StageAndScanner:
                    uint errorCode3 = num | syncAXIS.slsc_cfg_get_mode(this.handle, out this.oldOpMode) | syncAXIS.slsc_cfg_set_mode(this.handle, slsc_OperationMode.slsc_OperationMode_ScannerAndStage);
                    if (errorCode3 != 0U)
                    {
                        Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]:fail to slsc_cfg_get_mode : {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode3)), Array.Empty<object>());
                        return false;
                    }
                    break;
            }
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: list has began by {1}", (object)this.Index, (object)motionType), Array.Empty<object>());
            this.jobStatus.calcStatus = CalculationStatus.Unknown;
            this.jobStatus.transStatus = TransferStatus.Unknown;
            this.jobStatus.execStatus = ExecutionStatus.Unknown;
            uint errorCode4 = syncAXIS.slsc_list_begin(this.handle, out this.jobStatus.jobID);
            if (errorCode4 != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]:fail to slsc_list_begin : {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode4)), Array.Empty<object>());
                return false;
            }
            this.stream?.WriteLine("; LIST HAS BEGAN : " + DateTime.Now.ToString() + " with " + motionType.ToString());
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: list begin with job id : {1}", (object)this.Index, (object)this.jobStatus.jobID), Array.Empty<object>());
            return true;
        }

        /// <summary>리스트 명령 - 주파수, 펄스폭</summary>
        /// <param name="frequency">주파수 (Hz)</param>
        /// <param name="pulseWidth">펄스폭 (usec)</param>
        /// <returns></returns>
        public bool ListFrequency(float frequency, float pulseWidth)
        {
            if (this.handle == 0U || this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            uint errorCode = syncAXIS.slsc_list_set_laser_pulses(this.handle, 1.0 / (double)frequency / 2.0, (double)pulseWidth / 1000000.0, 0.0);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_list_set_laser_pulses: {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
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
            return true;
        }

        /// <summary>리스트 명령 - 속도</summary>
        /// <param name="jump">점프(jump 속도 (mm/s)</param>
        /// <param name="mark">마크(mark/arc) 속도 (mm/s)</param>
        /// <returns></returns>
        public bool ListSpeed(float jump, float mark)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(2U))
                return false;
            uint errorCode = 0U | syncAXIS.slsc_list_set_jump_speed(this.handle, (double)jump) | syncAXIS.slsc_list_set_mark_speed(this.handle, (double)mark);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_list_set_jump_speed/slsc_list_set_mark_speed : {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
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
            uint errorCode = 0U | syncAXIS.slsc_list_wait_with_laser_on(this.handle, (double)msec / 1000.0);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_list_wait_with_laser_on: {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
            this.stream?.WriteLine(string.Format("LASER_ON_DURING_MS = {0:F6}", (object)msec));
            return true;
        }

        /// <summary>리스트 명령 - 레이저 출사 시작</summary>
        /// <returns></returns>
        public bool ListLaserOn() => true;

        /// <summary>리스트 명령 - 레이저 출사 중지</summary>
        /// <returns></returns>
        public bool ListLaserOff() => true;

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
            if (!this.IsListReady(1U))
                return false;
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            uint errorCode = syncAXIS.slsc_list_jump_abs(this.handle, new double[2]
            {
        (double) vector2.X,
        (double) vector2.Y
            });
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_list_jump_abs : {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
            this.vPhysical = vector2;
            this.vLogical = vPosition;
            this.stream?.WriteLine(string.Format("JUMP_TO = {0:F3}, {1:F3}", (object)vector2.X, (object)vector2.Y));
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
            if (!this.IsListReady(1U))
                return false;
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            uint errorCode = syncAXIS.slsc_list_mark_abs(this.handle, new double[2]
            {
        (double) vector2.X,
        (double) vector2.Y
            });
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_list_mark_abs : {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
            this.vPhysical = vector2;
            this.vLogical = vPosition;
            this.stream?.WriteLine(string.Format("MARK_TO = {0:F3}, {1:F3}", (object)vector2.X, (object)vector2.Y));
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
            double[] Center = new double[2]
            {
        (double) vector2.X,
        (double) vector2.Y
            };
            float num1 = (float)Math.PI / 180f * sweepAngle;
            if (!this.IsListReady(1U))
                return false;
            uint errorCode = 0U | syncAXIS.slsc_list_circle_2d_abs(this.handle, Center, (double)num1);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: fail to slsc_list_circle_2d_abs : {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
            double num2 = 0.0;
            if ((double)this.vLogical.Y != (double)vCenter.Y || (double)this.vLogical.X != (double)vCenter.Y)
                num2 = Math.Atan2((double)this.vLogical.Y - (double)vCenter.Y, (double)this.vLogical.X - (double)vCenter.X);
            double num3 = Math.Sqrt(((double)vCenter.X - (double)this.vLogical.X) * ((double)vCenter.X - (double)this.vLogical.X) + ((double)vCenter.Y - (double)this.vLogical.Y) * ((double)vCenter.Y - (double)this.vLogical.Y));
            this.vLogical.X = (float)(num3 * Math.Cos(num2 + Math.PI / 180.0 * (double)sweepAngle)) + vCenter.X;
            this.vLogical.Y = (float)(num3 * Math.Sin(num2 + Math.PI / 180.0 * (double)sweepAngle)) + vCenter.Y;
            this.vPhysical = Vector2.Transform(this.vLogical, this.MatrixStack.ToResult);
            this.stream?.WriteLine(string.Format("ARC_BY_CENTER = {0:F3}, {1:F3}, SWEEP_ANGLE = {2:F3}", (object)vector2.X, (object)vector2.Y, (object)sweepAngle));
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
            bool flag1 = true & this.ListJump(Vector2.Transform(new Vector2(majorHalf * (float)Math.Cos((double)startAngle) + vCenter.X, minorHalf * (float)Math.Sin((double)startAngle) + vCenter.Y), Matrix3x2.CreateRotation((float)Math.PI / 180f * rotateAngle)), 1f);
            for (float num = 0.0f; (double)num < (double)sweepAngle; ++num)
            {
                Vector2 vPosition = Vector2.Transform(new Vector2(majorHalf * (float)Math.Cos((double)startAngle + (double)num * (double)Math.Sign(sweepAngle)) + vCenter.X, minorHalf * (float)Math.Sin((double)startAngle + (double)num * (double)Math.Sign(sweepAngle)) + vCenter.Y), Matrix3x2.CreateRotation((float)Math.PI / 180f * rotateAngle));
                flag1 &= this.ListJump(vPosition, 1f);
            }
            Vector2 vPosition1 = Vector2.Transform(new Vector2(majorHalf * (float)Math.Cos((double)startAngle + (double)sweepAngle) + vCenter.X, minorHalf * (float)Math.Sin((double)startAngle + (double)sweepAngle) + vCenter.Y), Matrix3x2.CreateRotation((float)Math.PI / 180f * rotateAngle));
            bool flag2 = flag1 & this.ListJump(vPosition1, 1f);
            this.stream?.WriteLine(string.Format("ELLIPSE_BY_CENTER = {0:F3}, {1:F3}, MAJOR = {2:F3}, MINOR = {3:F3}, START_ANGLE = {4:F3}, SWEEP_ANGLE = {5:F3}, ANGLE = {6:F3}", (object)vector2.X, (object)vector2.Y, (object)majorHalf, (object)minorHalf, (object)startAngle, (object)sweepAngle, (object)rotateAngle));
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

        /// <summary>
        /// 리스트 명령 - 확장 포트에 데이타 쓰기
        /// ExtDO16, ExtAO1, ExtAO2 만 지원됨
        /// </summary>
        /// <param name="ch">확장 커넥터 종류</param>
        /// <param name="value">값(16비트, 8비트, 2비트(int), 아나로그(float 10V)</param>
        /// <param name="compensator">compensator 보정용 객체</param>
        /// <returns></returns>
        public bool ListWriteData<T>(ExtensionChannel ch, T value, ICompensator<T> compensator = null)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            uint num1;
            switch (ch)
            {
                case ExtensionChannel.ExtDO2:
                    if (!this.IsListReady(1U))
                        return false;
                    int num2 = (int)(uint)Convert.ChangeType((object)value, typeof(uint));
                    return false;
                case ExtensionChannel.ExtDO8:
                    if (!this.IsListReady(1U))
                        return false;
                    int num3 = (int)(ushort)Convert.ChangeType((object)value, typeof(ushort));
                    return false;
                case ExtensionChannel.ExtDO16:
                    if (!this.IsListReady(1U))
                        return false;
                    num1 = syncAXIS.slsc_list_write_digital_out(this.handle, (ushort)Convert.ChangeType((object)value, typeof(ushort)), 0.0);
                    break;
                case ExtensionChannel.ExtAO1:
                    if (!this.IsListReady(1U))
                        return false;
                    double num4 = (double)Convert.ChangeType((object)value, typeof(double));
                    double num5 = (Math.Pow(2.0, 12.0) - 1.0) * num4 / 10.0;
                    num1 = syncAXIS.slsc_list_write_analog_x(this.handle, slsc_AnalogOutput.slsc_AnalogOutput_1, num4, 0.0);
                    break;
                case ExtensionChannel.ExtAO2:
                    if (!this.IsListReady(1U))
                        return false;
                    double num6 = (double)Convert.ChangeType((object)value, typeof(double));
                    double num7 = (Math.Pow(2.0, 12.0) - 1.0) * num6 / 10.0;
                    num1 = syncAXIS.slsc_list_write_analog_x(this.handle, slsc_AnalogOutput.slsc_AnalogOutput_2, num6, 0.0);
                    break;
                default:
                    return false;
            }
            this.stream?.WriteLine("DATA : " + ch.ToString() + " : " + value.ToString());
            return num1 == 0U;
        }

        /// <summary>확장1 포트의 16비트 디지털 출력의 특정 비트값을 변경</summary>
        /// <param name="bitPosition">0~15</param>
        /// <param name="onOff">출력</param>
        /// <returns></returns>
        public bool ListWriteExtDO16(ushort bitPosition, bool onOff)
        {
            if (this.handle == 0U || this.CtlGetStatus(RtcStatus.Aborted) || !this.IsListReady(1U))
                return false;
            ushort Mask = (ushort)(1U << (int)bitPosition);
            return syncAXIS.slsc_list_write_digital_out_mask(this.handle, onOff ? Mask : (ushort)0, Mask, 0.0) == 0U;
        }

        /// <summary>리스트 명령 끝 - 버퍼 닫기</summary>
        /// <returns></returns>
        /// s
        public bool ListEnd()
        {
            if (this.handle == 0U || this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            uint errorCode = syncAXIS.slsc_list_end(this.handle);
            if (errorCode != 0U)
            {
                Logger.Log(Logger.Type.Error, string.Format("{0} : fail to slsc_list_end : {1}", (object)this.Index, (object)this.CtlGetErrMsg(errorCode)), Array.Empty<object>());
                return false;
            }
            this.stream?.WriteLine("; LIST ENDED : " + DateTime.Now.ToString());
            this.stream?.WriteLine(Environment.NewLine);
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: list has ended. counts= {1}", (object)this.Index, (object)this.listTotalCount), Array.Empty<object>());
            return true;
        }

        /// <summary>리스트 명령 실행</summary>
        /// <param name="busyWait">모든 리스트 명령의 실행이 완료될때까지 대기</param>
        /// <returns></returns>
        public bool ListExecute(bool busyWait = true)
        {
            if (this.handle == 0U || this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            Stopwatch stopwatch = Stopwatch.StartNew();
            slsc_ExecState State = slsc_ExecState.slsc_ExecState_Idle;
            do
            {
                int execState = (int)syncAXIS.slsc_ctrl_get_exec_state(this.handle, out State);
                if (State != slsc_ExecState.slsc_ExecState_Executing)
                {
                    Thread.Sleep(10);
                    if (stopwatch.ElapsedMilliseconds > 3000L)
                    {
                        Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: trying to list execute but timed out !", (object)this.Index), Array.Empty<object>());
                        break;
                    }
                }
                else
                    break;
            }
            while (slsc_ExecState.slsc_ExecState_Executing != State);
            if (busyWait)
            {
                this.CtlBusyWait();
                int execState = (int)syncAXIS.slsc_ctrl_get_exec_state(this.handle, out State);
            }
            return true;
        }

        private bool IsDuplicated(Vector2 vPosition, float dz = 0.0f)
        {
            Vector2 vector2 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            return MathHelper.IsEqual(this.vPhysical.X, vector2.X) && MathHelper.IsEqual(this.vPhysical.Y, vector2.Y);
        }

        private bool IsListReady(uint count)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.listTotalCount += count;
            return true;
        }
    }
}
