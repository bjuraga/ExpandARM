namespace ExpandARM.Core.Exceptions
{
    public class SelfReferenceException : ExpandArmException
    {
        public SelfReferenceException(string message)
            : base(message)
        {
        }
    }
}
