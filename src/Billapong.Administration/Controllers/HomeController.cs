namespace Billapong.Administration.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// The home controller
    /// </summary>
    public class HomeController : Controller
    {
        // GET: /Home/

        /// <summary>
        /// The index action
        /// </summary>
        /// <returns>The view</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}