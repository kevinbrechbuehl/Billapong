namespace Billapong.Contract.Service
{
    using System;
    using System.ServiceModel;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Exceptions;

    /// <summary>
    /// Contract for the session management.
    /// </summary>
    [ServiceContract(Name = "Authentication", Namespace = Globals.ServiceContractNamespaceName)]
    public interface IAuthenticationService
    {
        /// <summary>
        /// Login to the service with specific user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="role">The role.</param>
        /// <returns>The session id.</returns>
        [OperationContract(Name = "Login")]
        [FaultContract(typeof(LoginFailedException))]
        Guid Login(string username, string password, Role role);

        /// <summary>
        /// Logouts the specified session id.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        [OperationContract(Name = "Logout")]
        void Logout(Guid sessionId);
    }
}
