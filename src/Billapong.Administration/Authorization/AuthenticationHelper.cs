namespace Billapong.Administration.Authorization
{
    using System;
    using System.Web;

    /// <summary>
    /// Helper class for authentication stuff
    /// </summary>
    public static class AuthenticationHelper
    {
        /// <summary>
        /// The session identifier key
        /// </summary>
        private const string SessionIdKey = "SessionId";

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public static Guid? SessionId
        {
            get
            {
                if (HttpContext.Current == null) return null;
                return HttpContext.Current.Session[SessionIdKey] as Guid?;
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session[SessionIdKey] = value;
                }
            }
        }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <returns>Session id or empty guid</returns>
        public static Guid GetSessionId()
        {
            return SessionId ?? Guid.Empty;
        }
    }
}