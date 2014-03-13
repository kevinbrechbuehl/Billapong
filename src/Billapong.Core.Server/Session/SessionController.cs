namespace Billapong.Core.Server.Session
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.ServiceModel;
    using System.Text;
    using System.Threading;
    using Billapong.Contract.Data.Session;
    using Billapong.Contract.Exceptions;
    using Billapong.DataAccess.Model.Session;
    using Billapong.DataAccess.Repository;

    public class SessionController
    {
        private const string Salt = "B1ll4p0ng";
        
        private readonly IRepository<User> userRepository; 

        #region Singleton Implementation

        /// <summary>
        /// Initializes static members of the <see cref="SessionController"/> class.
        /// </summary>
        static SessionController()
        {
            Current = new SessionController();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="SessionController"/> class from being created.
        /// </summary>
        private SessionController()
        {
            this.userRepository = new Repository<User>();
        }

        /// <summary>
        /// Gets the current instance.
        /// </summary>
        /// <value>
        /// The current instance.
        /// </value>
        public static SessionController Current { get; private set; }

        #endregion

        public Guid Login(string username, string password, Role role)
        {
            var hash = GetPasswordHash(password);
            var user = this.userRepository.Get(dbUser => dbUser.Username == username
                && dbUser.Password == hash
                && dbUser.Role == (int)role);

            if (!user.Any())
            {
                throw new FaultException<LoginFailedException>(new LoginFailedException(username), "Login failed");
            }

            return Guid.NewGuid();
        }

        public void Logout(Guid sessionId)
        {

        }

        public static string GetPasswordHash(string password)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                var stringBuilder = new StringBuilder();
                foreach (var element in hash)
                {
                    stringBuilder.Append(element.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}
