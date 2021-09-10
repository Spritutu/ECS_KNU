using System;


namespace Scanlab.Sirius.ClipperLib
{
    internal class ClipperException : Exception
    {
        public ClipperException(string description)
          : base(description)
        {
        }
    }
}
