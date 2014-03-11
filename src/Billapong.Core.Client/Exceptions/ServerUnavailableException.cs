namespace Billapong.Core.Client.Exceptions
{
    using System;

    public class ServerUnavailableException : Exception
    {
        public ServerUnavailableException(Exception inner) : base("Server unavailable", inner)
        {
        }
    }
}
