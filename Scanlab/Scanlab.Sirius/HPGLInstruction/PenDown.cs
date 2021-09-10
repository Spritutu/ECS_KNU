using System.Collections.Generic;

namespace Scanlab.Sirius.HPGLInstruction
{
    internal class PenDown : IInstruction
    {
        private PenDown()
        {
        }

        public static IEnumerable<IInstruction> Matches(string text)
        {
            if (text.StartsWith("PD"))
                yield return (IInstruction)new PenDown();
        }
    }
}