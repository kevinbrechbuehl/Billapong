namespace Billapong.Contract.Exceptions
{
    using System;

    /// <summary>
    /// Exception for invalid session for a authentication based service
    /// </summary>
    public class InvalidSessionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSessionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidSessionException(string message) : base(message)
        {
        }
    }
}
