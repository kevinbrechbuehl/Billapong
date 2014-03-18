using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billapong.Administration.Authorization
{
    using System.Web.Mvc;

    public class ServiceAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null || httpContext.Session == null) return false;
            return AuthenticationHelper.SessionId != null;
        } 
    }
}