
using Accord.Genetic;
using Accord.Math.Random;

namespace Scanlab.Sirius
{
    /// <summary>
    /// The chromosome is to solve TSP task (Travailing Salesman Problem).
    /// </summary>
    internal class TSPChromosome : PermutationChromosome
    {
        private double[,] map;

        /// <summary>Constructor</summary>
        public TSPChromosome(double[,] map)
          : base(map.GetLength(0))
        {
            this.map = map;
        }

        /// <summary>Copy Constructor</summary>
        protected TSPChromosome(TSPChromosome source)
          : base((PermutationChromosome)source)
        {
            this.map = source.map;
        }

        /// <summary>Create new random chromosome (factory method)</summary>
        public virtual IChromosome CreateNew() => (IChromosome)new TSPChromosome(this.map);

        /// <summary>Clone the chromosome</summary>
        public virtual IChromosome Clone() => (IChromosome)new TSPChromosome(this);

        /// <summary>Crossover operator</summary>
        public virtual void Crossover(IChromosome pair)
        {
            TSPChromosome tspChromosome = (TSPChromosome)pair;
            if (tspChromosome == null || ((ShortArrayChromosome)tspChromosome).Length != ((ShortArrayChromosome)this).Length)
                return;
            ushort[] child1 = new ushort[((ShortArrayChromosome)this).Length];
            ushort[] child2 = new ushort[((ShortArrayChromosome)this).Length];
            this.CreateChildUsingCrossover((ushort[])((ShortArrayChromosome)this).Value, (ushort[])((ShortArrayChromosome)tspChromosome).Value, child1);
            this.CreateChildUsingCrossover((ushort[])((ShortArrayChromosome)tspChromosome).Value, (ushort[])((ShortArrayChromosome)this).Value, child2);
        }

        private void CreateChildUsingCrossover(ushort[] parent1, ushort[] parent2, ushort[] child)
        {
            bool[] flagArray = new bool[((ShortArrayChromosome)this).Length];
            System.Random random = Generator.Random;
            int num1 = ((ShortArrayChromosome)this).Length - 1;
            ushort num2 = child[0] = parent2[0];
            flagArray[(int)num2] = true;
            for (int index1 = 1; index1 < ((ShortArrayChromosome)this).Length; ++index1)
            {
                int index2 = 0;
                while (index2 < num1 && (int)parent1[index2] != (int)num2)
                    ++index2;
                ushort num3 = index2 == num1 ? parent1[0] : parent1[index2 + 1];
                int index3 = 0;
                while (index3 < num1 && (int)parent2[index3] != (int)num2)
                    ++index3;
                ushort num4 = index3 == num1 ? parent2[0] : parent2[index3 + 1];
                bool flag1 = !flagArray[(int)num3];
                bool flag2 = !flagArray[(int)num4];
                if (flag1 & flag2)
                {
                    double num5 = this.map[(int)num3, 0] - this.map[(int)num2, 0];
                    double num6 = this.map[(int)num3, 1] - this.map[(int)num2, 1];
                    double num7 = this.map[(int)num4, 0] - this.map[(int)num2, 0];
                    double num8 = this.map[(int)num4, 1] - this.map[(int)num2, 1];
                    num2 = System.Math.Sqrt(num5 * num5 + num6 * num6) < System.Math.Sqrt(num7 * num7 + num8 * num8) ? num3 : num4;
                }
                else if (!(flag1 | flag2))
                {
                    int num5;
                    int index4 = num5 = random.Next((int)((ShortArrayChromosome)this).Length);
                    while (index4 < ((ShortArrayChromosome)this).Length && flagArray[index4])
                        ++index4;
                    if (index4 == ((ShortArrayChromosome)this).Length)
                    {
                        index4 = num5 - 1;
                        while (flagArray[index4])
                            --index4;
                    }
                    num2 = (ushort)index4;
                }
                else
                    num2 = flag1 ? num3 : num4;
                child[index1] = num2;
                flagArray[(int)num2] = true;
            }
        }
    }
}
