using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    internal struct UndoRedoEntityDeleteEntity
    {
        public int IndexLayer;
        public Layer Layer;
        public int IndexEntity;
        public IEntity Entity;
    }
}
