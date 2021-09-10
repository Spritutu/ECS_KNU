// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.TSPFitnessFunction
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Accord.Genetic;
using System;

namespace Scanlab.Sirius
{
    /// <summary>
    /// Fitness function for TSP task (Travaling Salasman Problem)
    /// </summary>
    internal class TSPFitnessFunction : IFitnessFunction
    {
        private double[,] map;

        public TSPFitnessFunction(double[,] map) => this.map = map;

        /// <summary>Evaluate chromosome - calculates its fitness value</summary>
        public double Evaluate(IChromosome chromosome) => 1.0 / (this.PathLength(chromosome) + 1.0);

        /// <summary>Translate genotype to phenotype</summary>
        public object Translate(IChromosome chromosome) => (object)chromosome.ToString();

        /// <summary>
        /// Calculate path length represented by the specified chromosome
        /// </summary>
        public double PathLength(IChromosome chromosome)
        {
            ushort[] numArray = ((ShortArrayChromosome)chromosome).Value;
            if (numArray.Length != this.map.GetLength(0))
                throw new ArgumentException("Invalid path specified - not all cities are visited");
            int index1 = (int)numArray[0];
            int index2 = (int)numArray[numArray.Length - 1];
            double num1 = this.map[index2, 0] - this.map[index1, 0];
            double num2 = this.map[index2, 1] - this.map[index1, 1];
            double num3 = Math.Sqrt(num1 * num1 + num2 * num2);
            int index3 = 1;
            for (int length = numArray.Length; index3 < length; ++index3)
            {
                int index4 = (int)numArray[index3];
                double num4 = this.map[index4, 0] - this.map[index1, 0];
                double num5 = this.map[index4, 1] - this.map[index1, 1];
                num3 += Math.Sqrt(num4 * num4 + num5 * num5);
                index1 = index4;
            }
            return num3;
        }
    }
}
