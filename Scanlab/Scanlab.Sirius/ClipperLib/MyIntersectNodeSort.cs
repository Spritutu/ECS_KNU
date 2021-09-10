using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius.ClipperLib
{
    internal class MyIntersectNodeSort : IComparer<IntersectNode>
    {
        public int Compare(IntersectNode node1, IntersectNode node2)
        {
            long num = node2.Pt.Y - node1.Pt.Y;
            if (num > 0L)
                return 1;
            return num < 0L ? -1 : 0;
        }
    }
}
