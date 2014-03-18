namespace Billapong.Administration.Controllers
{
    using System.Web.Mvc;
    using Billapong.Administration.Authorization;

    /// <summary>
    /// The home controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The index action
        /// </summary>
        /// <returns>The view</returns>
        [ServiceAuthorize]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}