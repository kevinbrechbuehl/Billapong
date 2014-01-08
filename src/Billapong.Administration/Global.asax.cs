namespace Billapong.Administration
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using App_Start;

    /// <summary>
    /// Global application file.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application start event.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Tracing.InitializeTracing();
        }

        /// <summary>
        /// Application end event.
        /// </summary>
        protected void Application_End()
        {
            Tracing.ShutdownTracing();
        }
    }
}
