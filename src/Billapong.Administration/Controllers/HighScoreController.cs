using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Billapong.Administration.Controllers
{
    using System.Net;
    using System.Threading.Tasks;

    using Billapong.Administration.Models.HighScore;

    using Core.Client.Tracing;
    using Service;

    public class HighScoreController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> MapHighScores()
        {
            try
            {
                await Tracer.Info("Refreshing highscores of all maps");

                var proxy = new AdministrationServiceClient();
                return this.PartialView("ScoresTable", new ScoresViewModel {ShowDetailColumn = true, Scores = proxy.GetMapHighScores()});
            }
            catch (Exception ex)
            {
                this.HandleException("Error while retrieving the highscores", ex);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public ActionResult Map(long id)
        {
            return this.View(new MapScoresViewModel { MapId = id });
        }

        public async Task<ActionResult> MapScores(long id)
        {
            try
            {
                await Tracer.Info(string.Format("Refreshing scores of map with id '{0}'", id));

                var proxy = new AdministrationServiceClient();
                return this.PartialView("ScoresTable", new ScoresViewModel { Scores = proxy.GetMapScores(id) });
            }
            catch (Exception ex)
            {
                this.HandleException(string.Format("Error while retrieving the scores for map '{0}'", id), ex);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
	}
}