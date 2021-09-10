
using Scanlab.Sirius.HPGLInstruction;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scanlab.Sirius
{
    internal class HpglFile
    {
        private string m_fileName;
        private double m_minX;
        private double m_maxX;
        private double m_minY;
        private double m_maxY;
        private double m_penDownLength;
        private double m_penUpLength;
        private List<IInstruction> m_instructions;
        private List<HpglError> m_errors;

        public HpglFile(string path)
        {
            this.m_instructions = new List<IInstruction>();
            this.m_errors = new List<HpglError>();
            this.m_fileName = Path.GetFileName(path);
            this.ReadWholeFile((TextReader)new StreamReader(path));
            this.SearchMinMax();
            this.MeasurePlotLength();
        }

        public string FileName => this.m_fileName;

        public IEnumerable<IInstruction> Instructions => (IEnumerable<IInstruction>)this.m_instructions;

        public IEnumerable<HpglError> Errors => (IEnumerable<HpglError>)this.m_errors;

        public double Width => this.m_maxX - this.m_minX;

        public double Height => this.m_maxY - this.m_minY;

        public double MinX => this.m_minX;

        public double MaxX => this.m_maxX;

        public double MinY => this.m_minY;

        public double MaxY => this.m_maxY;

        public double PenUpLength => this.m_penUpLength;

        public double PenDownLength => this.m_penDownLength;

        private void ReadWholeFile(TextReader file)
        {
            int lineNumber = 0;
            string lineText;
            while ((lineText = file.ReadLine()) != null)
            {
                ++lineNumber;
                this.ReadLine(lineNumber, lineText);
            }
        }

        private void ReadLine(int lineNumber, string lineText)
        {
            string str = lineText;
            char[] chArray = new char[1] { ';' };
            foreach (string text in str.Split(chArray))
            {
                if (!string.IsNullOrEmpty(text))
                    this.m_instructions.AddRange(this.ReadInstruction(lineNumber, text));
            }
        }

        private IEnumerable<IInstruction> ReadInstruction(
          int lineNumber,
          string text)
        {
            List<IInstruction> instructionList = new List<IInstruction>();
            instructionList.AddRange(PenUp.Matches(text));
            instructionList.AddRange(PenDown.Matches(text));
            instructionList.AddRange(PlotAbsolute.Matches(text));
            if (instructionList.Count != 0)
                return (IEnumerable<IInstruction>)instructionList;
            this.m_errors.Add(new HpglError(HpglErrorType.Warning, lineNumber, string.Format("Ignoring instruction \"{0}\"", (object)text)));
            return (IEnumerable<IInstruction>)instructionList;
        }

        private void SearchMinMax()
        {
            this.m_minX = 0.0;
            this.m_maxX = 0.0;
            this.m_minY = 0.0;
            this.m_maxY = 0.0;
            foreach (IInstruction instruction in this.Instructions)
            {
                if (instruction is PlotAbsolute plotAbsolute1)
                {
                    if (plotAbsolute1.X < this.m_minX)
                        this.m_minX = plotAbsolute1.X;
                    if (plotAbsolute1.X > this.m_maxX)
                        this.m_maxX = plotAbsolute1.X;
                    if (plotAbsolute1.Y < this.m_minY)
                        this.m_minY = plotAbsolute1.Y;
                    if (plotAbsolute1.Y > this.m_maxY)
                        this.m_maxY = plotAbsolute1.Y;
                }
            }
        }

        private void MeasurePlotLength()
        {
            bool flag = false;
            this.m_penDownLength = 0.0;
            foreach (IInstruction instruction in this.m_instructions)
            {
                switch (instruction)
                {
                    case PenDown _:
                        flag = true;
                        continue;
                    case PenUp _:
                        flag = false;
                        continue;
                    case PlotAbsolute _:
                        PlotAbsolute plotAbsolute = instruction as PlotAbsolute;
                        double num = Math.Sqrt(plotAbsolute.X * plotAbsolute.X + plotAbsolute.Y * plotAbsolute.Y);
                        if (flag)
                        {
                            this.m_penDownLength += num;
                            continue;
                        }
                        this.m_penUpLength += num;
                        continue;
                    default:
                        continue;
                }
            }
        }
    }
}
