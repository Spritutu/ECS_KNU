using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius.ClipperLib
{
    internal class OutPt
    {
        internal int Idx;
        internal IntPoint Pt;
        internal OutPt Next;
        internal OutPt Prev;
    }
}
