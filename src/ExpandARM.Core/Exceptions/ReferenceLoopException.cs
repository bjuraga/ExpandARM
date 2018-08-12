namespace ExpandARM.Core.Exceptions
{
    public class ReferenceLoopException : ExpandArmException
    {
        public ReferenceLoopException(string message) : base(message)
        {
        }
    }
}
