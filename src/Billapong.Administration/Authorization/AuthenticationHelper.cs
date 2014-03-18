using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billapong.Administration.Authorization
{
    public static class AuthenticationHelper
    {
        private const string SessionIdKey = "SessionId";
        
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

        public static Guid GetSessionId()
        {
            return SessionId ?? Guid.Empty;
        }
    }
}