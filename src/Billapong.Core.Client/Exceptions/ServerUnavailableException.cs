namespace Billapong.Core.Client.Exceptions
{
    using System;

    /// <summary>
    /// Exception for server unavailable.
    /// </summary>
    public class ServerUnavailableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerUnavailableException"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        public ServerUnavailableException(Exception inner) : base("Server unavailable", inner)
        {
        }
    }
}
