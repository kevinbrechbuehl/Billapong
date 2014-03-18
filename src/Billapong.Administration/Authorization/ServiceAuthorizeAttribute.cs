namespace Billapong.Administration.Authorization
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Custom attribute for authorization.
    /// </summary>
    public class ServiceAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Checks if session id is available in the session.
        /// </summary>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <returns>
        /// true if the user is authorized; otherwise, false.
        /// </returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null || httpContext.Session == null) return false;
            return AuthenticationHelper.SessionId != null;
        } 
    }
}