using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Billapong.Administration.Controllers
{
    using System.Net;
    using Core.Client.Tracing;
    using Service;

    public class GameController : Controller
    {
        public ActionResult Index()
        {
            Tracer.Info("Call Index() method on GameController");
            return View();
        }

        public ActionResult Games()
        {
            try
            {
                Tracer.Info("Call Games() method on GameController");
                Tracer.Debug("Refreshing games");
                
                var proxy = new AdministrationServiceClient();
                return this.PartialView(proxy.GetGames());
            }
            catch (Exception ex)
            {
                Tracer.Error("Error while retrieving the games", ex);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
	}
}