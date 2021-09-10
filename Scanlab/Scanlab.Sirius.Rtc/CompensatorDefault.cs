

namespace Scanlab.Sirius
{
    /// <summary>Compensator 기본 버전 (1 to 1)</summary>
    public class CompensatorDefault<T> : ICompensator<T>
    {
        /// <summary>동기화 객체</summary>
        public object SyncRoot { get; protected set; }

        /// <summary>식별자 (0, 1, 2, ...)</summary>
        public uint Index { get; private set; }

        /// <summary>이름</summary>
        public string Name { get; private set; }

        /// <summary>파일 이름</summary>
        public string FileName { get; private set; }

        /// <summary>생성자</summary>
        public CompensatorDefault() => this.SyncRoot = new object();

        /// <summary>생성자</summary>
        /// <param name="index">식별자</param>
        /// <param name="name">이름</param>
        public CompensatorDefault(uint index, string name)
          : this()
        {
            this.Index = index;
            this.Name = name;
        }

        /// <summary>보간 연산하기</summary>
        /// <param name="input">입력값</param>
        /// <param name="output">보간된 출력값</param>
        /// <returns></returns>
        public bool Interpolate(T input, out T output)
        {
            lock (this.SyncRoot)
                output = input;
            return true;
        }
    }
}
