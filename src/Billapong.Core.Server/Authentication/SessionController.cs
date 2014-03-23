namespace Billapong.Core.Server.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.ServiceModel;
    using System.Text;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Exceptions;
    using Billapong.DataAccess.Model.Session;
    using Billapong.DataAccess.Repository;

    /// <summary>
    /// Controller for session management
    /// </summary>
    public class SessionController
    {
        /// <summary>
        /// The salt for password hashing
        /// </summary>
        private const string Salt = "B1ll4p0ng";

        /// <summary>
        /// The lock object
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IRepository<User> userRepository;

        /// <summary>
        /// The session store
        /// </summary>
        private readonly IDictionary<Role, IList<Guid>> sessionStore; 

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
            this.sessionStore = new Dictionary<Role, IList<Guid>>
                                    {
                                        { Role.Administrator, new List<Guid>() },
                                        { Role.Editor, new List<Guid>() }
                                    };
        }

        /// <summary>
        /// Gets the current instance.
        /// </summary>
        /// <value>
        /// The current instance.
        /// </value>
        public static SessionController Current { get; private set; }

        #endregion

        /// <summary>
        /// Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="role">The role.</param>
        /// <returns>Session id for this login</returns>
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

            var sessionId = Guid.NewGuid();
            lock (LockObject)
            {
                this.sessionStore[role].Add(sessionId);    
            }
            
            return sessionId;
        }

        /// <summary>
        /// Logouts the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public void Logout(Guid sessionId)
        {
            lock (LockObject)
            {
                foreach (var store in this.sessionStore.Values)
                {
                    store.Remove(sessionId);
                }
            }
        }

        /// <summary>
        /// Determines whether The given session is valid.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="role">The role.</param>
        /// <returns>Boolean value if session is valid</returns>
        public bool IsValidSession(Guid sessionId, Role role)
        {
            lock (LockObject)
            {
                return this.sessionStore[role].Contains(sessionId);
            }
        }

        /// <summary>
        /// Gets the hash for a password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>Generated password hash</returns>
        private static string GetPasswordHash(string password)
        {
            var salted = string.Join("_", password, Salt);
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(salted));
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
