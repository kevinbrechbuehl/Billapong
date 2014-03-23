namespace Billapong.Core.Client.Authentication
{
    using System;

    /// <summary>
    /// Authentication provider to hold needed objects.
    /// </summary>
    public class AuthenticationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationProvider"/> class.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public AuthenticationProvider(Guid sessionId)
        {
            this.SessionId = sessionId;
        }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public Guid SessionId { get; private set; }
    }
}
