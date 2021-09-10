
using System.Collections.Generic;

namespace Scanlab.Sirius.HPGLInstruction
{
    internal class PenUp : IInstruction
    {
        private PenUp()
        {
        }

        public static IEnumerable<IInstruction> Matches(string text)
        {
            if (text.StartsWith("PU"))
                yield return (IInstruction)new PenUp();
        }
    }
}