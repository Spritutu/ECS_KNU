using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius.ClipperLib
{
    internal class LocalMinima
    {
        internal long Y;
        internal TEdge LeftBound;
        internal TEdge RightBound;
        internal LocalMinima Next;
    }
}
