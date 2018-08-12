using System;

namespace ExpandARM.Core.Exceptions
{
    public class ExpandArmException : Exception
    {
        public ExpandArmException()
            : base()
        {
        }

        public ExpandArmException(string message)
            : base(message)
        {
        }

        public ExpandArmException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
