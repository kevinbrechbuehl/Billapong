namespace Billapong.Core.Server.Services
{
    using System;
    using System.ServiceModel;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Service;
    using Billapong.Core.Server.Authentication;

    /// <summary>
    /// Implementation of the session management service.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AuthenticationService : IAuthenticationService
    {
        /// <summary>
        /// Login to the service with specific user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="role">The role.</param>
        /// <returns>
        /// The session id.
        /// </returns>
        public Guid Login(string username, string password, Role role)
        {
            return SessionController.Current.Login(username, password, role);
        }

        /// <summary>
        /// Logouts the specified session id.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public void Logout(Guid sessionId)
        {
            SessionController.Current.Logout(sessionId);
        }
    }
}
