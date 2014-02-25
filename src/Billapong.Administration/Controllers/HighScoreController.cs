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

    public class HighScoreController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MapHighScores()
        {
            try
            {
                Tracer.Debug("Refreshing highscores of all maps");

                var proxy = new AdministrationServiceClient();
                return this.PartialView(proxy.GetMapHighScores());
            }
            catch (Exception ex)
            {
                Tracer.Error("Error while retrieving the highscores", ex);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public ActionResult Map(long id)
        {
            // todo (breck1): implement action
            
            return null;
        }
	}
}