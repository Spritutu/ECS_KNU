
using Scanlab.Sirius.HPGLInstruction;
using System.Collections.Generic;

namespace Scanlab.Sirius
{
    internal class HPGLHelper
    {
        private HpglFile hpgl;
        private LwPolyline polyline;

        public HPGLHelper(string fileName) => this.hpgl = new HpglFile(fileName);

        public List<IEntity> Parse()
        {
            if (this.hpgl == null)
                return (List<IEntity>)null;
            if (this.hpgl.Instructions == null)
                return (List<IEntity>)null;
            List<IEntity> entityList = new List<IEntity>();
            bool flag = false;
            double num1 = 0.0;
            double num2 = 0.0;
            foreach (IInstruction instruction in this.hpgl.Instructions)
            {
                switch (instruction)
                {
                    case PenDown _:
                        if (!flag)
                        {
                            this.polyline = new LwPolyline();
                            this.polyline.Add(new LwPolyLineVertex((float)HpglUnit.PluToMm(num1), (float)HpglUnit.PluToMm(num2)));
                        }
                        flag = true;
                        continue;
                    case PenUp _:
                        if (flag)
                        {
                            if (this.polyline.Count >= 2)
                            {
                                this.polyline.Regen();
                                entityList.Add((IEntity)this.polyline);
                            }
                            this.polyline = (LwPolyline)null;
                        }
                        flag = false;
                        continue;
                    case PlotAbsolute _:
                        PlotAbsolute plotAbsolute = instruction as PlotAbsolute;
                        if (flag)
                            this.polyline.Add(new LwPolyLineVertex((float)HpglUnit.PluToMm(plotAbsolute.X), (float)HpglUnit.PluToMm(plotAbsolute.Y)));
                        num1 = plotAbsolute.X;
                        num2 = plotAbsolute.Y;
                        continue;
                    default:
                        continue;
                }
            }
            if (this.polyline != null)
            {
                this.polyline.Regen();
                entityList.Add((IEntity)this.polyline);
                this.polyline = (LwPolyline)null;
            }
            return entityList;
        }
    }
}
