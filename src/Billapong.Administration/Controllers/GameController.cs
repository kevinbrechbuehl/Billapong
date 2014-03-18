namespace Billapong.Administration.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Billapong.Administration.Authorization;
    using Core.Client.Tracing;
    using Service;

    /// <summary>
    /// Game MVC controller
    /// </summary>
    public class GameController : ControllerBase
    {
        /// <summary>
        /// Index action of the controller.
        /// </summary>
        /// <returns>The view.</returns>
        [ServiceAuthorize]
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Action for returning current games.
        /// </summary>
        /// <returns>The view with all the games</returns>
        [ServiceAuthorize]
        public async Task<ActionResult> Games()
        {
            try
            {
                await Tracer.Debug("Refreshing games");

                var proxy = new AdministrationServiceClient(AuthenticationHelper.GetSessionId());
                return this.PartialView(proxy.GetGames());
            }
            catch (Exception ex)
            {
                this.HandleException("Error while retrieving the games", ex);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}