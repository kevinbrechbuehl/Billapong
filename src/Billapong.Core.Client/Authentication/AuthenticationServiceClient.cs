namespace Billapong.Core.Client.Authentication
{
    using System;
    using System.Threading.Tasks;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Service;

    public class AuthenticationServiceClient : RichClientBase<IAuthenticationService>, IAuthenticationService
    {
        public Guid Login(string username, string password, Role role)
        {
            return this.Execute(() => this.Proxy.Login(username, password, role));
        }

        public void Logout(Guid sessionId)
        {
            this.Execute(() => this.Proxy.Logout(sessionId));
        }

        public async Task<Guid> LoginAsync(string username, string password, Role role)
        {
            return await this.ExecuteAsync(() => this.Proxy.Login(username, password, role));
        }

        public async Task LogoutAsync(Guid sessionId)
        {
            await this.ExecuteAsync(() => this.Proxy.Logout(sessionId));
        }
    }
}
