namespace Billapong.Core.Client.Authentication
{
    using System;

    public class AuthenticationProvider
    {
        public Guid SessionId { get; private set; }

        public AuthenticationProvider(Guid sessionId)
        {
            this.SessionId = sessionId;
        }
    }
}
