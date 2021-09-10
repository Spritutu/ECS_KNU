using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius.ClipperLib
{
    internal class OutRec
    {
        internal int Idx;
        internal bool IsHole;
        internal bool IsOpen;
        internal OutRec FirstLeft;
        internal OutPt Pts;
        internal OutPt BottomPt;
        internal PolyNode PolyNode;
    }
}
