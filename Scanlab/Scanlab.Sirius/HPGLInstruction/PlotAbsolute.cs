
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scanlab.Sirius.HPGLInstruction
{
    internal class PlotAbsolute : IInstruction
    {
        private double m_x;
        private double m_y;
        private static Regex m_regex = new Regex("^P[AUD](?:(?<x>-?\\d+(?:\\.\\d+)?)(?:,(?<y>-?\\d+(?:\\.\\d+)?)?),?)*$", RegexOptions.ExplicitCapture);

        private PlotAbsolute(double x, double y)
        {
            this.m_x = x;
            this.m_y = y;
        }

        public double X => this.m_x;

        public double Y => this.m_y;

        public static IEnumerable<IInstruction> Matches(string instruction)
        {
            if (instruction == "PA")
            {
                yield return (IInstruction)new PlotAbsolute(0.0, 0.0);
            }
            else
            {
                Match m = PlotAbsolute.m_regex.Match(instruction);
                if (m.Success)
                {
                    int plotCount = m.Groups["x"].Captures.Count;
                    for (int plotIndex = 0; plotIndex < plotCount; ++plotIndex)
                        yield return (IInstruction)new PlotAbsolute(double.Parse(m.Groups["x"].Captures[plotIndex].Value), m.Groups["y"].Captures.Count <= plotIndex ? 0.0 : double.Parse(m.Groups["y"].Captures[plotIndex].Value));
                }
                m = (Match)null;
            }
        }
    }
}
