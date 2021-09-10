
using System;
using System.IO;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>RTC 가상 객체</summary>
    public class RtcVirtual : IRtc, IDisposable, IRtcExtension
    {
        private string[] ctbFileName = new string[4];
        protected StreamWriter stream;
        protected string outputFileName;
        protected bool aborted;
        protected Vector2 vPhysical;
        protected Vector2 vLogical;
        private bool disposed;
        private Vector2 _pixelDelta;
        private uint _pixelIndex;
        private uint _pixelCount;
        private Vector2 _pixelStart;

        /// <summary>점프 위치 이벤트</summary>
        public event RtcJumpTo OnJumpTo;

        /// <summary>마크 위치 이벤트</summary>
        public event RtcMarkTo OnMarkTo;

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

        /// <summary>두번째 스캐너 헤드 보정 테이블 번호 (1~4)</summary>
        public CorrectionTableIndex SecondaryHeadTable { get; private set; }

        /// <summary>생성자</summary>
        public RtcVirtual()
        {
            this.MatrixStack = (IMatrixStack)new Scanlab.Sirius.MatrixStack();
            this.vPhysical = Vector2.Zero;
            this.vLogical = Vector2.Zero;
        }

        /// <summary>생성자</summary>
        /// <param name="index">식별 번호 (1,2,3,...)</param>
        /// <param name="outputFileName">RTC 데이타 상세 로그 출력 파일</param>
        public RtcVirtual(uint index, string outputFileName = "")
          : this()
        {
            this.Index = index;
            this.outputFileName = outputFileName;
        }

        ~RtcVirtual()
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

        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
                this.stream?.Dispose();
            this.disposed = true;
        }

        public bool Initialize(float kFactor, LaserMode laserMode, string ctbFileName)
        {
            this.CtlLoadCorrectionFile(CorrectionTableIndex.Table1, ctbFileName);
            this.LaserMode = laserMode;
            this.Fpk = 0.0f;
            return true;
        }

        public bool CtlLoadCorrectionFile(CorrectionTableIndex tableIndex, string ctbFileName)
        {
            this.ctbFileName[(int)tableIndex] = ctbFileName;
            return true;
        }

        public bool CtlSelectCorrection(
          CorrectionTableIndex primaryHeadTableIndex,
          CorrectionTableIndex secondaryHeadTableIndex = CorrectionTableIndex.None)
        {
            return true;
        }

        public bool CtlLaserOn()
        {
            this.stream?.WriteLine("LASER ON");
            return true;
        }

        public bool CtlLaserOff()
        {
            this.stream?.WriteLine("LASER OFF");
            return true;
        }

        public bool CtlMove(Vector2 vPosition)
        {
            this.stream?.WriteLine(string.Format("MOVE TO = {0:F3}, {1} mm", (object)vPosition, (object)vPosition.Y));
            return true;
        }

        public bool CtlMove(float x, float y) => this.CtlMove(new Vector2(0.0f, 0.0f));

        public bool CtlFirstPulseKiller(float usec)
        {
            this.Fpk = usec;
            this.stream?.WriteLine(string.Format("FIRST PULSE KILLER = {0:F3} usec", (object)usec));
            return true;
        }

        public bool CtlLaserSignalLevel(ILaserControlSignal laserControlSignal) => true;

        public bool CtlExternalControl(IRtcExternalControlMode mode, uint maxStartCounts = 0) => true;

        /// <summary>외부 /START 실행된 회수 조회</summary>
        /// <param name="counts">회수값</param>
        /// <returns></returns>
        public bool CtlExternalStartCounts(out uint counts)
        {
            counts = 0U;
            return true;
        }

        public bool CtlFrequency(float frequency, float pulseWidth) => true;

        public bool CtlDelay(
          float laserOn,
          float laserOff,
          float scannerJump,
          float scannerMark,
          float scannerPolygon)
        {
            return true;
        }

        public bool CtlSpeed(float jump, float mark) => true;

        public bool CtlWriteData<T>(ExtensionChannel ch, T value, ICompensator<T> compensator = null) => true;

        public bool CtlWriteExtDO16(ushort bitPosition, bool onOff)
        {
            this.stream?.WriteLine(string.Format("DATA : ExtDO16 : BIT{0} : {1}", (object)bitPosition, (object)onOff.ToString()));
            return true;
        }

        public bool CtlReadData<T>(ExtensionChannel ch, out T value, ICompensator<T> compensator = null)
        {
            value = default(T);
            return true;
        }

        public bool CtlGetStatus(RtcStatus status)
        {
            bool flag = false;
            switch (status)
            {
                case RtcStatus.Busy:
                    flag = false;
                    break;
                case RtcStatus.NotBusy:
                    flag = true;
                    break;
                case RtcStatus.List1Busy:
                    flag = false;
                    break;
                case RtcStatus.List2Busy:
                    flag = false;
                    break;
                case RtcStatus.NoError:
                    flag = true;
                    break;
                case RtcStatus.Aborted:
                    flag = this.aborted;
                    break;
                case RtcStatus.PositionAckOK:
                    flag = true;
                    break;
                case RtcStatus.PowerOK:
                    flag = true;
                    break;
                case RtcStatus.TempOK:
                    flag = true;
                    break;
            }
            return flag;
        }

        public string CtlGetErrMsg(uint errorCode) => string.Empty;

        public bool CtlBusyWait() => true;

        public bool CtlAbort()
        {
            this.aborted = true;
            this.stream?.Flush();
            this.stream?.Dispose();
            this.stream = (StreamWriter)null;
            return true;
        }

        public bool CtlReset()
        {
            this.aborted = false;
            return true;
        }

        public bool ListBegin(ILaser laser, ListType listType = ListType.Auto)
        {
            if (!string.IsNullOrEmpty(this.outputFileName))
            {
                this.stream?.Dispose();
                this.stream = new StreamWriter(this.outputFileName);
            }
            this.aborted = false;
            this.stream?.WriteLine("; LIST HAS BEGAN : " + DateTime.Now.ToString() + " with " + listType.ToString());
            return true;
        }

        public bool ListFrequency(float frequency, float pulseWidth)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.stream?.WriteLine(string.Format("FREQUENCY_HZ = {0:F3}", (object)frequency));
            this.stream?.WriteLine(string.Format("PULSE_WIDTH_US = {0:F3}", (object)pulseWidth));
            return true;
        }

        public bool ListFirstPulseKiller(float usec)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.Fpk = usec;
            this.stream?.WriteLine(string.Format("FIRST PULSE KILLER = {0:F3} usec", (object)usec));
            return true;
        }

        public bool ListDelay(
          float laserOn,
          float laserOff,
          float scannerJump,
          float scannerMark,
          float scannerPolygon)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.stream?.WriteLine(string.Format("LASER_ON_DELAY_US = {0:F3}", (object)laserOn));
            this.stream?.WriteLine(string.Format("LASER_OFF_DELAY_US = {0:F3}", (object)laserOff));
            this.stream?.WriteLine(string.Format("SCANNER_JUMP_DELAY_US = {0:F3}", (object)scannerJump));
            this.stream?.WriteLine(string.Format("SCANNER_MARK_DELAY_US = {0:F3}", (object)scannerMark));
            this.stream?.WriteLine(string.Format("SCANNER_POLYGON_DELAY_US = {0:F3}", (object)scannerPolygon));
            return true;
        }

        public bool ListSpeed(float jump, float mark)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.stream?.WriteLine(string.Format("SCANNER_JUMP_SPEED_MM_S = {0:F3}", (object)jump));
            this.stream?.WriteLine(string.Format("SCANNER_MARK_SPEED_MM_S = {0:F3}", (object)mark));
            return true;
        }

        public bool ListWait(float msec)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.stream?.WriteLine(string.Format("WAIT_DURING_MS = {0:F6}", (object)msec));
            return true;
        }

        public bool ListLaserOn(float msec)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            RtcMarkTo onMarkTo = this.OnMarkTo;
            if (onMarkTo != null)
                onMarkTo((object)this, this.vPhysical);
            this.stream?.WriteLine(string.Format("LASER_ON_DURING_MS = {0:F6}", (object)msec));
            return true;
        }

        public bool ListLaserOn()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.stream?.WriteLine("LASER_ON");
            return true;
        }

        public bool ListLaserOff()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.stream?.WriteLine("LASER_OFF");
            return true;
        }

        public bool ListJump(Vector2 vPosition, float rampFactor = 1f)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if (this.IsDuplicated(vPosition))
                return true;
            Vector2 v1 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            int num1 = (int)((double)Vector2.Distance(v1, this.vPhysical) / 0.2);
            if (num1 <= 0)
                num1 = 1;
            if (1 == num1)
            {
                RtcJumpTo onJumpTo = this.OnJumpTo;
                if (onJumpTo != null)
                    onJumpTo((object)this, v1);
            }
            else
            {
                float num2 = 1f / (float)num1;
                for (float num3 = 0.0f; (double)num3 < 1.0; num3 += num2)
                {
                    if (this.CtlGetStatus(RtcStatus.Aborted))
                        return false;
                    Vector2 v2 = (1f - num3) * this.vPhysical + num3 * v1;
                    RtcJumpTo onJumpTo = this.OnJumpTo;
                    if (onJumpTo != null)
                        onJumpTo((object)this, v2);
                    num2 *= 1.3f;
                }
                RtcJumpTo onJumpTo1 = this.OnJumpTo;
                if (onJumpTo1 != null)
                    onJumpTo1((object)this, v1);
            }
            this.vPhysical = v1;
            this.vLogical = vPosition;
            this.stream?.WriteLine(string.Format("JUMP_TO = {0:F3}, {1:F3}", (object)v1.X, (object)v1.Y));
            return true;
        }

        public bool ListJump(float x, float y, float rampFactor = 1f) => this.ListJump(new Vector2(x, y), rampFactor);

        public bool ListMark(Vector2 vPosition, float rampFactor = 1f)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if (this.IsDuplicated(vPosition))
                return true;
            Vector2 v1 = Vector2.Transform(vPosition, this.MatrixStack.ToResult);
            int num1 = (int)((double)Vector2.Distance(v1, this.vPhysical) / 0.2);
            if (num1 <= 0)
                num1 = 1;
            if (1 == num1)
            {
                RtcMarkTo onMarkTo = this.OnMarkTo;
                if (onMarkTo != null)
                    onMarkTo((object)this, v1);
            }
            else
            {
                float num2 = 1f / (float)num1;
                for (float num3 = 0.0f; (double)num3 < 1.0; num3 += num2)
                {
                    if (this.CtlGetStatus(RtcStatus.Aborted))
                        return false;
                    Vector2 v2 = (1f - num3) * this.vPhysical + num3 * v1;
                    RtcMarkTo onMarkTo = this.OnMarkTo;
                    if (onMarkTo != null)
                        onMarkTo((object)this, v2);
                    num2 *= 1.2f;
                }
                RtcMarkTo onMarkTo1 = this.OnMarkTo;
                if (onMarkTo1 != null)
                    onMarkTo1((object)this, v1);
            }
            this.vPhysical = v1;
            this.vLogical = vPosition;
            this.stream?.WriteLine(string.Format("MARK_TO = {0:F3}, {1:F3}", (object)v1.X, (object)v1.Y));
            return true;
        }

        public bool ListMark(float x, float y, float rampFactor = 1f) => this.ListMark(new Vector2(x, y), rampFactor);

        public bool ListArc(Vector2 vCenter, float sweepAngle)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            Vector2 vector2 = Vector2.Transform(vCenter, this.MatrixStack.ToResult);
            Quaternion rotation;
            Matrix4x4.Decompose(new Matrix4x4(this.MatrixStack.ToResult), out Vector3 _, out rotation, out Vector3 _);
            double num1 = Math.Acos((double)rotation.W) * 2.0 * 57.2957801818848;
            double num2 = 0.0;
            if ((double)this.vLogical.Y != (double)vCenter.Y || (double)this.vLogical.X != (double)vCenter.Y)
                num2 = Math.Atan2((double)this.vLogical.Y - (double)vCenter.Y, (double)this.vLogical.X - (double)vCenter.X);
            double num3 = Math.Sqrt(((double)vCenter.X - (double)this.vLogical.X) * ((double)vCenter.X - (double)this.vLogical.X) + ((double)vCenter.Y - (double)this.vLogical.Y) * ((double)vCenter.Y - (double)this.vLogical.Y));
            if (this.OnMarkTo != null && num3 > 0.200000002980232)
            {
                double num4 = num2 * 57.2957801818848 + num1;
                double num5 = 5.0;
                if ((double)sweepAngle > 0.0)
                {
                    for (double num6 = num4 + num5; num6 < num4 + (double)sweepAngle; num6 += num5)
                    {
                        double num7 = num3 * Math.Cos(num6 * (Math.PI / 180.0)) + (double)vector2.X;
                        double num8 = num3 * Math.Sin(num6 * (Math.PI / 180.0)) + (double)vector2.Y;
                        if (this.CtlGetStatus(RtcStatus.Aborted))
                            return false;
                        RtcMarkTo onMarkTo = this.OnMarkTo;
                        if (onMarkTo != null)
                            onMarkTo((object)this, new Vector2((float)num7, (float)num8));
                    }
                }
                else
                {
                    for (double num6 = num4 - num5; num6 > num4 + (double)sweepAngle; num6 -= num5)
                    {
                        double num7 = num3 * Math.Cos(num6 * (Math.PI / 180.0)) + (double)vector2.X;
                        double num8 = num3 * Math.Sin(num6 * (Math.PI / 180.0)) + (double)vector2.Y;
                        if (this.CtlGetStatus(RtcStatus.Aborted))
                            return false;
                        RtcMarkTo onMarkTo = this.OnMarkTo;
                        if (onMarkTo != null)
                            onMarkTo((object)this, new Vector2((float)num7, (float)num8));
                    }
                }
                double num9 = num3 * Math.Cos((num4 + (double)sweepAngle) * (Math.PI / 180.0)) + (double)vector2.X;
                double num10 = num3 * Math.Sin((num4 + (double)sweepAngle) * (Math.PI / 180.0)) + (double)vector2.Y;
                RtcMarkTo onMarkTo1 = this.OnMarkTo;
                if (onMarkTo1 != null)
                    onMarkTo1((object)this, new Vector2((float)num9, (float)num10));
            }
            this.vLogical.X = (float)(num3 * Math.Cos(num2 + Math.PI / 180.0 * (double)sweepAngle)) + vCenter.X;
            this.vLogical.Y = (float)(num3 * Math.Sin(num2 + Math.PI / 180.0 * (double)sweepAngle)) + vCenter.Y;
            this.vPhysical = Vector2.Transform(this.vLogical, this.MatrixStack.ToResult);
            this.stream?.WriteLine(string.Format("ARC_BY_CENTER = {0:F3}, {1:F3}, SWEEP_ANGLE = {2:F3}", (object)vector2.X, (object)vector2.Y, (object)sweepAngle));
            return true;
        }

        public bool ListArc(float cx, float cy, float rampFactor = 1f) => this.ListArc(new Vector2(cx, cy), rampFactor);

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

        public bool ListWriteData<T>(ExtensionChannel ch, T value, ICompensator<T> compensator = null)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.stream?.WriteLine("DATA : " + ch.ToString() + " : " + value.ToString());
            return true;
        }

        public bool ListWriteExtDO16(ushort bitPosition, bool onOff)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.stream?.WriteLine(string.Format("DATA : ExtDO16 : BIT{0} : {1}", (object)bitPosition, (object)onOff.ToString()));
            return true;
        }

        public bool ListSkyWriting(float laserOnShift_usec, float timeLag_usec, float angularLimit)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.stream?.WriteLine(string.Format("SWK WRITING. ONSHIFT= {0:F3} LAG= {1:F3} A.LIMIT= {2:F3}", (object)laserOnShift_usec, (object)timeLag_usec, (object)angularLimit));
            return true;
        }

        public bool ListPixelLine(float usec, Vector2 vDelta, uint pixelCount, ExtensionChannel ext = ExtensionChannel.ExtAO2)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this._pixelDelta = vDelta;
            this._pixelCount = pixelCount;
            this._pixelIndex = 0U;
            this._pixelStart = this.vLogical;
            this.vLogical = this._pixelStart + Vector2.Multiply(vDelta, (float)pixelCount);
            this.vPhysical = Vector2.Transform(this.vLogical, this.MatrixStack.ToResult);
            this.stream?.WriteLine(string.Format("PIXEL LINE. PEROID= {0:F3} {1} COUNT= {2}", (object)usec, (object)ext.ToString(), (object)pixelCount));
            return true;
        }

        public bool ListPixel(float usec, float weight = 0.0f, ICompensator<float> compensator = null)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if ((double)usec > 0.0)
            {
                Vector2 v = Vector2.Transform(this._pixelStart + Vector2.Multiply(this._pixelDelta, (float)this._pixelIndex), this.MatrixStack.ToResult);
                RtcMarkTo onMarkTo = this.OnMarkTo;
                if (onMarkTo != null)
                    onMarkTo((object)this, v);
            }
            ++this._pixelIndex;
            return this._pixelIndex <= this._pixelCount;
        }

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
            return !this.CtlGetStatus(RtcStatus.Aborted);
        }

        public bool ListWobbel(float amplitudeX, float amplitudeY, float frequencyHz)
        {
            this.stream?.WriteLine(string.Format("WOBBEL= {0:F3}, {1:F3} Hz= {2:F3}", (object)amplitudeX, (object)amplitudeX, (object)frequencyHz));
            return true;
        }

        public bool ListEnd()
        {
            this.stream?.WriteLine("; LIST ENDED : " + DateTime.Now.ToString());
            return true;
        }

        public bool ListExecute(bool busyWait = true)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.stream?.WriteLine("; LIST EXECUTED : " + DateTime.Now.ToString());
            this.stream?.WriteLine(Environment.NewLine);
            this.stream?.Flush();
            this.stream?.Dispose();
            this.stream = (StreamWriter)null;
            return true;
        }

        protected bool IsDuplicated(Vector2 vPosition)
        {
            Matrix4x4 matrix = new Matrix4x4(this.MatrixStack.ToResult);
            Vector2 vector2 = Vector2.Transform(vPosition, matrix);
            return MathHelper.IsEqual(this.vPhysical.X, vector2.X) && MathHelper.IsEqual(this.vPhysical.Y, vector2.Y);
        }
    }
}
