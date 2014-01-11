namespace Billapong.Contract.Exceptions
{
    using System;

    public class GameNotOpenException : Exception
    {
        public GameNotOpenException()
        {
        }

        public GameNotOpenException(string message) : base(message)
        {
        }
    }
}
