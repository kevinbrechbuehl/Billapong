namespace Billapong.Administration.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// The tracing controller
    /// </summary>
    public class TracingController : Controller
    {
        // GET: /Tracing/

        /// <summary>
        /// The index action
        /// </summary>
        /// <returns>The result.</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}