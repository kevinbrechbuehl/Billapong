namespace Billapong.Core.Client.Session
{
    using System;
    using System.Threading.Tasks;
    using Billapong.Contract.Data.Session;
    using Billapong.Contract.Service;

    public class SessionServiceClient : RichClientBase<ISessionService>, ISessionService
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
