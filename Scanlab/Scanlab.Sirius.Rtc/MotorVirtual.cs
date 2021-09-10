
namespace Scanlab.Sirius
{
    /// <summary>Motor Virtual</summary>
    public class MotorVirtual : IMotor
    {
        /// <summary>동기화 객체</summary>
        public object SyncRoot { get; protected set; }

        /// <summary>모터 번호</summary>
        public uint No { get; set; }

        /// <summary>모터 이름</summary>
        public string Name { get; set; }

        /// <summary>위치 (mm)</summary>
        public float Position { get; protected set; }

        /// <summary>준비상태 여부</summary>
        public bool IsReady { get; protected set; }

        /// <summary>구동중 여부</summary>
        public bool IsBusy { get; protected set; }

        /// <summary>알람 발생 여부</summary>
        public bool IsError { get; protected set; }

        /// <summary>사용자 정의 데이타</summary>
        public object Tag { get; set; }

        /// <summary>생성자</summary>
        public MotorVirtual()
        {
            this.SyncRoot = new object();
            this.Name = "Motor Virtual ";
            this.Position = -1f;
            this.IsReady = false;
            this.IsBusy = false;
            this.IsError = true;
        }

        /// <summary>생성자</summary>
        /// <param name="no">축 번호</param>
        /// <param name="name">축 이름</param>
        public MotorVirtual(uint no, string name)
          : this()
        {
            this.No = no;
            this.Name = name;
        }

        /// <summary>홈(원점) 검색 시작</summary>
        /// <returns></returns>
        public bool CtlHomeSearch()
        {
            lock (this.SyncRoot)
            {
                this.IsReady = true;
                this.IsBusy = false;
                this.Position = 0.0f;
                return true;
            }
        }

        /// <summary>절대 위치로 이동</summary>
        /// <param name="position">위치 (mm)</param>
        /// <returns></returns>
        public bool CtlMoveAbs(float position)
        {
            lock (this.SyncRoot)
            {
                this.Position = position;
                return true;
            }
        }

        /// <summary>상대 위치 이동</summary>
        /// <param name="distance">거리값(mm)</param>
        /// <returns></returns>
        public bool CtlMoveRel(float distance)
        {
            lock (this.SyncRoot)
            {
                this.Position += distance;
                return true;
            }
        }

        /// <summary>정지</summary>
        /// <returns></returns>
        public bool CtlMoveStop()
        {
            lock (this.SyncRoot)
            {
                this.IsReady = false;
                return true;
            }
        }

        /// <summary>알람 해제</summary>
        /// <returns></returns>
        public bool CtlReset()
        {
            lock (this.SyncRoot)
            {
                this.IsReady = true;
                this.IsError = false;
                return true;
            }
        }
    }
}
