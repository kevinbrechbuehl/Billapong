using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Billapong.Administration.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Client.Tracing;
    using Service;

    public class GameController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Games()
        {
            try
            {
                await Tracer.Debug("Refreshing games");
                
                var proxy = new AdministrationServiceClient();
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