namespace Billapong.Core.Client.Authentication
{
    using System;
    using System.Threading.Tasks;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Service;

    /// <summary>
    /// Service proxy for authentication wcf service.
    /// </summary>
    public class AuthenticationServiceClient : RichClientBase<IAuthenticationService>, IAuthenticationService
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
            return this.Execute(() => this.Proxy.Login(username, password, role));
        }

        /// <summary>
        /// Logouts the specified session id.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public void Logout(Guid sessionId)
        {
            this.Execute(() => this.Proxy.Logout(sessionId));
        }

        /// <summary>
        /// Login to the service with specific user asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="role">The role.</param>
        /// <returns>
        /// The session id task.
        /// </returns>
        public async Task<Guid> LoginAsync(string username, string password, Role role)
        {
            return await this.ExecuteAsync(() => this.Proxy.Login(username, password, role));
        }

        /// <summary>
        /// Logouts the specified session id asnchronous.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>Async task</returns>
        public async Task LogoutAsync(Guid sessionId)
        {
            await this.ExecuteAsync(() => this.Proxy.Logout(sessionId));
        }
    }
}
