using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab
{
    /// <summary>비트 조작</summary>
    /// <typeparam name="TData">데이타 타입 : byte, short, int32, int64</typeparam>
    public sealed class BitHelper<TData> where TData : struct
    {
        /// <summary>데이타 접근요</summary>
        public TData Data { get; set; }

        /// <summary>생성자</summary>
        public BitHelper() => this.Data = default(TData);

        /// <summary>생성자 (초기 데이타)</summary>
        /// <param name="data"></param>
        public BitHelper(TData data) => this.Data = data;

        /// <summary>비트 인덱스로 접근/조작</summary>
        /// <param name="bit">비트위치</param>
        /// <returns></returns>
        public bool this[int bit]
        {
            get
            {
                long num = (long)(1 << bit);
                return Convert.ToBoolean(Convert.ToInt64((object)this.Data) & num);
            }
            set
            {
                long num = (long)(1 << bit);
                long int64 = Convert.ToInt64((object)this.Data);
                this.Data = (TData)Convert.ChangeType((object)(!Convert.ToBoolean(value) ? int64 & ~num : int64 | num), typeof(TData));
            }
        }

        /// <summary>복합 비트 인덱스들로 접근/조작</summary>
        /// <param name="bits">비트 배열</param>
        /// <returns></returns>
        public bool this[int[] bits]
        {
            get
            {
                bool flag = true;
                foreach (int bit in bits)
                    flag &= this[bit];
                return flag;
            }
            set
            {
                foreach (int bit in bits)
                    this[bit] = value;
            }
        }
    }

    /// <summary>비트 조작용 객체</summary>
    /// <typeparam name="TEnum">각 비트 필드에 대한 설명을 가진 열거 데이타</typeparam>
    /// <typeparam name="TData">데이타 타입 : 최대 64비트 (byte, short, int32, int64 등)</typeparam>
    public sealed class BitHelper<TEnum, TData>
      where TEnum : Enum
      where TData : struct
    {
        /// <summary>데이타 접근용</summary>
        public TData Data { get; set; }

        /// <summary>생성자</summary>
        public BitHelper() => this.Data = default(TData);

        /// <summary>생성자 (데이타로 초기화)</summary>
        /// <param name="data"></param>
        public BitHelper(TData data) => this.Data = data;

        /// <summary>해당 비트 설정</summary>
        /// <param name="bit"></param>
        public void Add(TEnum bit) => this[bit] = true;

        /// <summary>해당 비트 해제</summary>
        /// <param name="bit"></param>
        public void Remove(TEnum bit) => this[bit] = false;

        /// <summary>해당 비트 설정 여부</summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        public bool Contain(TEnum bit) => this[bit];

        /// <summary>해당 비트 접근 및 조작</summary>
        /// <param name="bit">플래그 비트 위치</param>
        /// <returns></returns>
        public bool this[TEnum bit]
        {
            get
            {
                long num = (long)(1 << Convert.ToInt32((object)bit));
                return Convert.ToBoolean(Convert.ToInt64((object)this.Data) & num);
            }
            set
            {
                long num = (long)(1 << Convert.ToInt32((object)bit));
                long int64 = Convert.ToInt64((object)this.Data);
                this.Data = (TData)Convert.ChangeType((object)(!Convert.ToBoolean(value) ? int64 & ~num : int64 | num), typeof(TData));
            }
        }

        /// <summary>복합 비트 접근/조작</summary>
        /// <param name="bits">비트 배열</param>
        /// <returns></returns>
        public bool this[TEnum[] bits]
        {
            get
            {
                bool flag = true;
                foreach (TEnum bit in bits)
                    flag &= this[bit];
                return flag;
            }
            set
            {
                foreach (TEnum bit in bits)
                    this[bit] = value;
            }
        }

        /// <summary>해당 비트 접근/조작</summary>
        /// <param name="bit">비트 위치</param>
        /// <returns></returns>
        public bool this[int bit]
        {
            get
            {
                long num = (long)(1 << bit);
                return Convert.ToBoolean(Convert.ToInt64((object)this.Data) & num);
            }
            set
            {
                long num = (long)(1 << bit);
                long int64 = Convert.ToInt64((object)this.Data);
                this.Data = (TData)Convert.ChangeType((object)(!Convert.ToBoolean(value) ? int64 & ~num : int64 | num), typeof(TData));
            }
        }

        /// <summary>복합 비트 인덱스들로 접근/조작</summary>
        /// <param name="bits">비트 배열</param>
        /// <returns></returns>
        public bool this[int[] bits]
        {
            get
            {
                bool flag = true;
                foreach (int bit in bits)
                    flag &= this[bit];
                return flag;
            }
            set
            {
                foreach (int bit in bits)
                    this[bit] = value;
            }
        }
    }
}
