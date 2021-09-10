
using System;
using System.Collections.Generic;
using System.Threading;

namespace Scanlab.Sirius
{
    /// <summary>가공량 통지용 콜백</summary>
    /// <param name="sender">IMarker 인터페이스</param>
    /// <param name="markerArg">IMarkerArg 인터페이스 (마커 가공 인자)</param>
    public delegate void MarkerEventHandler(IMarker sender, IMarkerArg markerArg);

    /// <summary>마커 객체 (기본 버전)</summary>
    public class MarkerDefault : IMarker
    {
        protected IDocument clonedDoc;
        protected Thread thread;
        /// <summary>
        /// 진행률 계산을 위한 전체 인덱스 개수 (인덱스 = 오프셋 개수 * 레이어 개수 * 레이어내 엔티티 수)
        /// </summary>
        protected int progressTotal;
        /// <summary>진행률 계산을 위한 현재 인덱스 위치</summary>
        protected int progressIndex;
        /// <summary>가공 시작전 RTC 의 행렬 스택 복사본</summary>
        protected IMatrixStack oldMatrixStack;

        /// <summary>진행률 이벤트 핸들러</summary>
        public event MarkerEventHandler OnProgress;

        /// <summary>완료 이벤트 핸들러</summary>
        public event MarkerEventHandler OnFinished;

        /// <summary>식별 번호</summary>
        public uint Index { get; set; }

        /// <summary>이름</summary>
        public string Name { get; set; }

        /// <summary>가공 준비 상태</summary>
        public virtual bool IsReady => this.clonedDoc != null && this.clonedDoc.Layers.Count != 0 && (this.MarkerArg != null && this.MarkerArg.Rtc.CtlGetStatus(RtcStatus.NoError)) && (!this.MarkerArg.Laser.IsError && !this.IsBusy);

        /// <summary>출사중 여부</summary>
        public virtual bool IsBusy
        {
            get
            {
                bool flag = false;
                if (this.MarkerArg != null && this.MarkerArg.Rtc != null)
                    flag |= this.MarkerArg.Rtc.CtlGetStatus(RtcStatus.Busy);
                if (this.MarkerArg != null && this.MarkerArg.Laser != null)
                    flag |= this.MarkerArg.Laser.IsBusy;
                if (this.thread != null)
                    flag |= this.thread.IsAlive;
                return flag;
            }
        }

        /// <summary>에러 여부</summary>
        public virtual bool IsError
        {
            get
            {
                bool flag = false;
                if (this.MarkerArg != null && this.MarkerArg.Rtc != null)
                    flag |= !this.MarkerArg.Rtc.CtlGetStatus(RtcStatus.NoError);
                if (this.MarkerArg != null && this.MarkerArg.Laser != null)
                    flag |= this.MarkerArg.Laser.IsError;
                return flag;
            }
        }

        /// <summary>마커 시작시 전달 인자 (Ready 에 의해 업데이트 되고, Start 시 내부적으로 사용됨)</summary>
        public virtual IMarkerArg MarkerArg { get; protected set; }

        /// <summary>
        /// 스캐너가 회전되어 장착되어 있는 경우 설정. 기본값 (0)
        /// 지정된 각도만큼 내부에서 회전 처리됨
        /// </summary>
        public virtual double ScannerRotateAngle { get; set; }

        /// <summary>Ready 에 의해 복제된 문서(IDocument) 객체</summary>
        public IDocument Document => this.clonedDoc;

        /// <summary>사용자 정의 데이타</summary>
        public object Tag { get; set; }

        /// <summary>생성자</summary>
        /// <param name="index">식별 번호</param>
        public MarkerDefault(uint index)
        {
            this.Index = index;
            this.Name = "default";
            this.MarkerArg = (IMarkerArg)new MarkerArgDefault();
            this.ScannerRotateAngle = 0.0;
        }

        /// <summary>생성자</summary>
        /// <param name="index">식별 번호</param>
        /// <param name="name">이름</param>
        public MarkerDefault(uint index, string name)
          : this(index)
        {
            this.Name = name;
        }

        /// <summary>
        /// 마커는 내부 쓰레드에 의해 가공 데이타를 처리하게 되는데, 이때 가공 데이타(IDocument)에
        /// 크로스 쓰레드 상태가 될수있으므로, 준비(Prepare)시에는 가공 데이타를 모두 복제(Clone) 하여 가공시
        /// 데이타에 대한 쓰레드 안전 접근을 처리하게 된다. 또한 가공중 뷰에 의해 원본 데이타가 조작, 수정되더라도
        /// 준비(Ready) 즉 신규 데이타를 다운로드하지 않으면 아무런 영향이 없게 된다.
        /// </summary>
        /// <param name="markerArg">가공 인자</param>
        /// <returns></returns>
        public virtual bool Ready(IMarkerArg markerArg)
        {
            if (markerArg == null || markerArg.Document == null || (markerArg.Rtc == null || markerArg.Laser == null))
                return false;
            this.MarkerArg = markerArg;
            this.clonedDoc = (IDocument)this.MarkerArg.Document.Clone();
            IRtc rtc = this.MarkerArg.Rtc;
            RtcCharacterSetHelper.Clear(rtc);
            bool flag = true;
            for (int index = 0; index < this.clonedDoc.Layers.Count; ++index)
            {
                Layer layer = this.clonedDoc.Layers[index];
                if (layer.IsMarkerable)
                {
                    foreach (IEntity entity in (ObservableList<IEntity>)layer)
                    {
                        if (entity is SiriusText siriusText5)
                            flag &= siriusText5.RegisterCharacterSetIntoRtc(rtc);
                        if (entity is Text text5)
                            flag &= text5.RegisterCharacterSetIntoRtc(rtc);
                    }
                }
            }
            if (!flag)
                Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: fail to register character into rtc", (object)this.Index, (object)this.Name), Array.Empty<object>());
            this.MarkerArg.Progress = 0.0;
            this.OnProgressing();
            if (flag)
                Logger.Log(Logger.Type.Info, string.Format("marker [{0}] {1}: document has been ready with offset: {2}", (object)this.Index, (object)this.Name, (object)this.clonedDoc.RotateOffset.ToString()), Array.Empty<object>());
            return flag;
        }

        /// <summary>복제된 문서 데이타를 초기화 (다시 Ready 를 호출하여 문서 복제 필요)</summary>
        /// <returns></returns>
        public virtual bool Clear()
        {
            if (this.IsBusy)
            {
                Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: trying to clear but busy", (object)this.Index, (object)this.Name), Array.Empty<object>());
                return false;
            }
            this.clonedDoc.New();
            Logger.Log(Logger.Type.Info, string.Format("marker [{0}] {1}: cleared", (object)this.Index, (object)this.Name), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 가공 시작
        /// 내부 함수 호출 순서
        /// WorkerThread : PreWork -&gt; MainWork (LayerWork * N  -&gt; EntityWork * N) -&gt; PostWork
        /// </summary>
        /// <returns></returns>
        public virtual bool Start()
        {
            Logger.Log(Logger.Type.Info, string.Format("marker [{0}] {1}: trying to start", (object)this.Index, (object)this.Name), Array.Empty<object>());
            if (this.MarkerArg == null || this.MarkerArg.Rtc == null || this.MarkerArg.Laser == null)
            {
                Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: invalid marker arg, rtc, laser status", (object)this.Index, (object)this.Name), Array.Empty<object>());
                return false;
            }
            IRtc rtc = this.MarkerArg.Rtc;
            ILaser laser = this.MarkerArg.Laser;
            IMotor motorZ = this.MarkerArg.MotorZ;
            if (rtc.CtlGetStatus(RtcStatus.Busy))
            {
                Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: busy now !", (object)this.Index, (object)this.Name), Array.Empty<object>());
                return false;
            }
            if (!rtc.CtlGetStatus(RtcStatus.PowerOK))
                Logger.Log(Logger.Type.Warn, string.Format("marker [{0}] {1}: scanner supply power is not ok !", (object)this.Index, (object)this.Name), Array.Empty<object>());
            if (!rtc.CtlGetStatus(RtcStatus.PositionAckOK))
                Logger.Log(Logger.Type.Warn, string.Format("marker [{0}] {1}: scanner position ack is not ok !", (object)this.Index, (object)this.Name), Array.Empty<object>());
            if (!rtc.CtlGetStatus(RtcStatus.NoError))
            {
                Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: rtc has a internal problem !", (object)this.Index, (object)this.Name), Array.Empty<object>());
                return false;
            }
            if (laser.IsError)
            {
                Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: laser source has a error status !", (object)this.Index, (object)this.Name), Array.Empty<object>());
                return false;
            }
            if (motorZ != null && (!motorZ.IsReady || motorZ.IsBusy || motorZ.IsError))
            {
                Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: motor z invalid status", (object)this.Index, (object)this.Name), Array.Empty<object>());
                return false;
            }
            if (this.thread != null && this.thread.IsAlive)
                return false;
            if (this.MarkerArg.Offsets.Count == 0)
            {
                this.MarkerArg.Offsets.Add(Offset.Zero);
                Logger.Log(Logger.Type.Warn, string.Format("marker [{0}] {1}: no offset information ... so reset to origin 0, 0", (object)this.Index, (object)this.Name), Array.Empty<object>());
            }
            Logger.Log(Logger.Type.Debug, string.Format("marker [{0}] {1}: total offset counts = {2}", (object)this.Index, (object)this.Name, (object)this.MarkerArg.Offsets.Count), Array.Empty<object>());
            if (this.clonedDoc == null || this.clonedDoc.Layers.Count == 0)
            {
                Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: document doesn't has any layers", (object)this.Index, (object)this.Name), Array.Empty<object>());
                return false;
            }
            this.thread = new Thread(new ThreadStart(this.WorkerThread));
            this.thread.Name = "Marker: " + this.Name;
            this.thread.Priority = ThreadPriority.Normal;
            this.thread.Start();
            return true;
        }

        /// <summary>가공 강제 정지</summary>
        /// <returns></returns>
        public virtual bool Stop()
        {
            if (this.MarkerArg == null)
                return false;
            Logger.Log(Logger.Type.Warn, string.Format("marker [{0}] {1}: trying to stop ...", (object)this.Index, (object)this.Name), Array.Empty<object>());
            this.MarkerArg.Rtc?.CtlAbort();
            this.MarkerArg.Rtc?.CtlLaserOff();
            if (this.thread == null)
                return false;
            if (!this.thread.Join(2000))
                Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: fail to join with thread", (object)this.Index, (object)this.Name), Array.Empty<object>());
            this.thread = (Thread)null;
            return true;
        }

        /// <summary>리셋 (에러 상태 해제 시도)</summary>
        /// <returns></returns>
        public virtual bool Reset()
        {
            if (this.MarkerArg == null)
                return false;
            this.MarkerArg.Rtc?.CtlReset();
            this.MarkerArg.Laser?.CtlReset();
            if (!this.IsBusy)
            {
                this.MarkerArg.Progress = 0.0;
                this.OnProgressing();
            }
            return true;
        }

        protected virtual void OnProgressing()
        {
            Delegate[] invocationList = this.OnProgress?.GetInvocationList();
            if (invocationList == null)
                return;
            foreach (MarkerEventHandler markerEventHandler in invocationList)
                markerEventHandler((IMarker)this, this.MarkerArg);
        }

        protected virtual void OnFinishing()
        {
            Delegate[] invocationList = this.OnFinished?.GetInvocationList();
            if (invocationList == null)
                return;
            foreach (MarkerEventHandler markerEventHandler in invocationList)
                markerEventHandler((IMarker)this, this.MarkerArg);
        }

        protected virtual void WorkerThread()
        {
            this.MarkerArg.StartTime = DateTime.Now;
            IRtc rtc = this.MarkerArg.Rtc;
            bool success = this.PreWork() && this.MainWork();
            this.MarkerArg.IsSuccess = success;
            this.PostWork(success);
        }

        /// <summary>
        /// 사전 작업
        /// 순서 : PreWork -&gt; MainWork (LayerWork * N  -&gt; EntityWork * N) -&gt; PostWork
        /// </summary>
        protected virtual bool PreWork()
        {
            this.oldMatrixStack = (IMatrixStack)this.MarkerArg.Rtc.MatrixStack.Clone();
            int num = 0;
            List<Offset> offsets = this.MarkerArg.Offsets;
            foreach (Layer layer in (ObservableList<Layer>)this.clonedDoc.Layers)
                num += layer.Count;
            this.progressTotal = offsets.Count * num;
            this.progressIndex = 0;
            this.MarkerArg.Progress = 0.0;
            this.MarkerArg.IsSuccess = false;
            this.OnProgressing();
            return true;
        }

        /// <summary>
        /// 주 작업
        /// 순서 : PreWork -&gt; MainWork (LayerWork * N  -&gt; EntityWork * N) -&gt; PostWork
        /// </summary>
        protected virtual bool MainWork()
        {
            bool flag = true;
            IRtc rtc = this.MarkerArg.Rtc;
            List<Offset> offsets = this.MarkerArg.Offsets;
            double scannerRotateAngle = this.ScannerRotateAngle;
            if (this.MarkerArg.IsExternalStart && 1 != this.clonedDoc.Layers.Count)
                Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: external start option is valid only single layer document", (object)this.Index, (object)this.Name), Array.Empty<object>());
            for (int index1 = 0; index1 < offsets.Count; ++index1)
            {
                Offset offset = offsets[index1];
                Logger.Log(Logger.Type.Debug, string.Format("marker [{0}] {1}: offset [{2}] : {3}", (object)this.Index, (object)this.Name, (object)index1, (object)offset.ToString()), Array.Empty<object>());
                rtc.MatrixStack.Push(scannerRotateAngle);
                IMatrixStack matrixStack1 = rtc.MatrixStack;
                double x1 = (double)offset.X;
                Offset rotateOffset = this.clonedDoc.RotateOffset;
                double x2 = (double)rotateOffset.X;
                double dx = x1 + x2;
                double y1 = (double)offset.Y;
                rotateOffset = this.clonedDoc.RotateOffset;
                double y2 = (double)rotateOffset.Y;
                double dy = y1 + y2;
                matrixStack1.Push(dx, dy);
                IMatrixStack matrixStack2 = rtc.MatrixStack;
                double angle1 = (double)offset.Angle;
                rotateOffset = this.clonedDoc.RotateOffset;
                double angle2 = (double)rotateOffset.Angle;
                double angle3 = angle1 + angle2;
                matrixStack2.Push(angle3);
                for (int index2 = 0; index2 < this.clonedDoc.Layers.Count; ++index2)
                {
                    Layer layer = this.clonedDoc.Layers[index2];
                    flag &= this.LayerWork(index1, index2, layer);
                    if (!flag)
                        break;
                }
                rtc.MatrixStack.Pop();
                rtc.MatrixStack.Pop();
                rtc.MatrixStack.Pop();
                if (!flag)
                    break;
            }
            return flag;
        }

        /// <summary>레이어 가공 (매 레이어 가공시 호출됨)</summary>
        /// <param name="i">오프셋 번호</param>
        /// <param name="j">레이어 번호</param>
        /// <param name="layer">레이어 객체</param>
        /// <returns></returns>
        protected virtual bool LayerWork(int i, int j, Layer layer)
        {
            if (!layer.IsMarkerable)
                return true;
            bool flag = true;
            IRtc rtc = this.MarkerArg.Rtc;
            ListType listType = this.MarkerArg.RtcListType;
            if (this.MarkerArg.IsExternalStart)
                listType = ListType.Single;
            IRtcAutoLaserControl autoLaserControl = rtc as IRtcAutoLaserControl;
            ILaser laser = this.MarkerArg.Laser;
            IMotor motorZ = this.MarkerArg.MotorZ;
            switch (layer.MotionType)
            {
                case MotionType.ScannerOnly:
                case MotionType.StageOnly:
                case MotionType.StageAndScanner:
                    if (motorZ != null)
                    {
                        flag = flag & motorZ.IsReady & !motorZ.IsBusy & !motorZ.IsError;
                        if (flag)
                            flag = flag & motorZ.CtlMoveAbs(layer.ZPosition) & MathHelper.IsEqual(motorZ.Position, layer.ZPosition);
                        if (!flag)
                            Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: motor Z invalid position/status", (object)this.Index, (object)this.Name), Array.Empty<object>());
                    }
                    if (flag)
                    {
                        if (autoLaserControl != null && layer.IsALC)
                        {
                            autoLaserControl.AutoLaserControlByPositionFileName = layer.AlcPositionFileName;
                            autoLaserControl.AutoLaserControlByPositionTableNo = layer.AlcPositionTableNo;
                            switch (layer.AlcSignal)
                            {
                                case AutoLaserControlSignal.ExtDO8Bit:
                                case AutoLaserControlSignal.ExtDO16:
                                    flag &= autoLaserControl.CtlAutoLaserControl<uint>(layer.AlcSignal, layer.AlcMode, (uint)layer.AlcPercentage100, (uint)layer.AlcMinValue, (uint)layer.AlcMaxValue);
                                    break;
                                default:
                                    flag &= autoLaserControl.CtlAutoLaserControl<float>(layer.AlcSignal, layer.AlcMode, (float)(uint)layer.AlcPercentage100, (float)(uint)layer.AlcMinValue, (float)(uint)layer.AlcMaxValue);
                                    break;
                            }
                        }
                        Logger.Log(Logger.Type.Debug, string.Format("marker [{0}] {1}: layer {2} has started", (object)this.Index, (object)this.Name, (object)layer.Name), Array.Empty<object>());
                        flag &= rtc.ListBegin(laser, listType);
                        for (int index = 0; index < layer.Count; ++index)
                        {
                            IEntity entity = layer[index];
                            flag &= this.EntityWork(i, j, index, entity);
                            if (flag)
                            {
                                this.MarkerArg.Progress = (double)this.progressIndex / (double)this.progressTotal * 100.0;
                                this.OnProgressing();
                                ++this.progressIndex;
                            }
                            else
                                break;
                        }
                        if (flag)
                            flag &= rtc.ListEnd();
                        if (flag && !this.MarkerArg.IsExternalStart)
                            flag &= rtc.ListExecute();
                        Logger.Log(Logger.Type.Debug, string.Format("marker [{0}] {1}: layer {2} has ended", (object)this.Index, (object)this.Name, (object)layer.Name), Array.Empty<object>());
                        break;
                    }
                    break;
            }
            return flag;
        }

        /// <summary>엔티티 가공 (레이어 내의 매 엔티티 마다 호출됨)</summary>
        /// <param name="i">오프셋 번호</param>
        /// <param name="j">레이어 번호</param>
        /// <param name="k">엔티티 번호</param>
        /// <param name="entity">엔티티 객체</param>
        /// <returns></returns>
        protected virtual bool EntityWork(int i, int j, int k, IEntity entity)
        {
            bool flag = true;
            if (entity is IMarkerable markerable)
            {
                switch (this.MarkerArg.MarkTargets)
                {
                    case MarkTargets.All:
                    case MarkTargets.Custom:
                        flag &= markerable.Mark(this.MarkerArg);
                        break;
                    case MarkTargets.Selected:
                        if (!entity.IsSelected)
                        {
                            switch (entity)
                            {
                                case IPen _:
                                case PenReturn _:
                                    break;
                                default:
                                    goto label_11;
                            }
                        }
                        flag &= markerable.Mark(this.MarkerArg);
                        break;
                    case MarkTargets.SelectedButBoundRect:
                        if (entity is IPen || entity is PenReturn)
                        {
                            flag &= markerable.Mark(this.MarkerArg);
                            break;
                        }
                        if (entity.IsSelected)
                        {
                            uint repeat = entity.BoundRect.Repeat;
                            entity.BoundRect.Repeat = this.MarkerArg.RepeatSelectedButBoundRect;
                            flag &= entity.BoundRect.Mark(this.MarkerArg);
                            entity.BoundRect.Repeat = repeat;
                            break;
                        }
                        break;
                    default:
                        flag = false;
                        break;
                }
            }
        label_11:
            return flag;
        }

        /// <summary>
        /// 마무리 작업
        /// 순서 : PreWork -&gt; MainWork (LayerWork * N  -&gt; EntityWork * N) -&gt; PostWork
        /// </summary>
        /// <returns></returns>
        protected virtual bool PostWork(bool success)
        {
            IRtc rtc = this.MarkerArg.Rtc;
            rtc.MatrixStack = this.oldMatrixStack;
            this.MarkerArg.EndTime = DateTime.Now;
            TimeSpan timeSpan = this.MarkerArg.EndTime - this.MarkerArg.StartTime;
            this.MarkerArg.IsSuccess = success;
            if (success)
            {
                if (this.MarkerArg.IsExternalStart)
                {
                    IRtcExtension rtcExtension = rtc as IRtcExtension;
                    switch (rtc)
                    {
                        case Rtc4 _:
                            Rtc4ExternalControlMode empty1 = Rtc4ExternalControlMode.Empty;
                            empty1.Add(Rtc4ExternalControlMode.Bit.ExternalStart);
                            empty1.Add(Rtc4ExternalControlMode.Bit.ExternalStartAgain);
                            success &= rtcExtension.CtlExternalControl((IRtcExternalControlMode)empty1);
                            break;
                        case Rtc5 _:
                            Rtc5ExternalControlMode empty2 = Rtc5ExternalControlMode.Empty;
                            empty2.Add(Rtc5ExternalControlMode.Bit.ExternalStart);
                            empty2.Add(Rtc5ExternalControlMode.Bit.ExternalStartAgain);
                            empty2.Add(Rtc5ExternalControlMode.Bit.ExternalStop);
                            empty2.Add(Rtc5ExternalControlMode.Bit.TrackDelay);
                            success &= rtcExtension.CtlExternalControl((IRtcExternalControlMode)empty2);
                            break;
                        case Rtc6 _:
                            Rtc6ExternalControlMode empty3 = Rtc6ExternalControlMode.Empty;
                            empty3.Add(Rtc6ExternalControlMode.Bit.ExternalStart);
                            empty3.Add(Rtc6ExternalControlMode.Bit.ExternalStartAgain);
                            empty3.Add(Rtc6ExternalControlMode.Bit.ExternalStop);
                            empty3.Add(Rtc6ExternalControlMode.Bit.TrackDelay);
                            success &= rtcExtension.CtlExternalControl((IRtcExternalControlMode)empty3);
                            break;
                    }
                }
                this.MarkerArg.Progress = 100.0;
                Logger.Log(Logger.Type.Info, string.Format("marker [{0}] {1}: job finished. time= {2:F3}s", (object)this.Index, (object)this.Name, (object)timeSpan.TotalSeconds), Array.Empty<object>());
            }
            else
                Logger.Log(Logger.Type.Error, string.Format("marker [{0}] {1}: aborted or fail to mark", (object)this.Index, (object)this.Name), Array.Empty<object>());
            this.OnFinishing();
            return true;
        }
    }
}
