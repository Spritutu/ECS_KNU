
using System;
using System.Collections.Generic;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 블럭 집합 객체
    /// Autocad의 블럭을 리스트로 관리
    /// </summary>
    public class Blocks : List<Block>, ICloneable
    {
        private IDocument owner;

        /// <summary>부모 문서</summary>
        public IDocument Owner
        {
            get => this.owner;
            internal set => this.owner = value;
        }

        /// <summary>생성자</summary>
        public Blocks()
        {
        }

        /// <summary>생성자</summary>
        /// <param name="owner"></param>
        public Blocks(IDocument owner) => this.owner = owner;

        /// <summary>이름으로 블럭 검색</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Block NameOf(string name) => this[this.FindIndex((Predicate<Block>)(x => x.Name == name))];

        /// <summary>블럭들을 모두 복제</summary>
        /// <returns></returns>
        public object Clone()
        {
            Blocks blocks = new Blocks();
            foreach (Block block in (List<Block>)this)
                blocks.Add((Block)block.Clone());
            return (object)blocks;
        }
    }
}
