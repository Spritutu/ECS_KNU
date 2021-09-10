using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    internal class JobStatus
    {
        public uint jobID;

        public CalculationStatus calcStatus { get; set; }

        public TransferStatus transStatus { get; set; }

        public ExecutionStatus execStatus { get; set; }
    }
}
