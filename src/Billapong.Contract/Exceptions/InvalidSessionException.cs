namespace Billapong.Contract.Exceptions
{
    using System;

    public class InvalidSessionException : Exception
    {
        public InvalidSessionException(string message) : base(message)
        {
        }
    }
}
