using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Recipe.Model
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RECIPE_STEP
    {
        public int STEP_ID { get; set; } 
        public double X_POS { get; set; }
        public double Y_POS { get; set; }
        public double Z_POS { get; set; }
        public double T_POS { get; set; }
        public double R_POS { get; set; }
        public double POWER_PERCENT { get; set; }
        public int REPEAT { get; set; }
        public string SCAN_FILE { get; set; }
    }
}
