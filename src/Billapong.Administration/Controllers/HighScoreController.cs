namespace Billapong.Administration.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Billapong.Administration.Authorization;
    using Billapong.Administration.Models.HighScore;
    using Core.Client.Tracing;
    using Service;

    /// <summary>
    /// MVC controller for the highscore tables.
    /// </summary>
    public class HighScoreController : ControllerBase
    {
        /// <summary>
        /// Action for returning overview.
        /// </summary>
        /// <returns>The view.</returns>
        [ServiceAuthorize]
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Action for returning all map high scores.
        /// </summary>
        /// <returns>View with highscores per map</returns>
        [ServiceAuthorize]
        public async Task<ActionResult> MapHighScores()
        {
            try
            {
                await Tracer.Info("Refreshing highscores of all maps");

                var proxy = new AdministrationServiceClient(AuthenticationHelper.GetSessionId());
                return this.PartialView("ScoresTable", new ScoresViewModel { ShowDetailColumn = true, Scores = proxy.GetMapHighScores() });
            }
            catch (Exception ex)
            {
                this.HandleException("Error while retrieving the highscores", ex);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Action for returning score of specific map.
        /// </summary>
        /// <param name="id">The map identifier.</param>
        /// <returns>The view</returns>
        [ServiceAuthorize]
        public ActionResult Map(long id)
        {
            return this.View(new MapScoresViewModel { MapId = id });
        }

        /// <summary>
        /// Action for returning scores of specific map.
        /// </summary>
        /// <param name="id">The map identifier.</param>
        /// <returns>View with all scores as a table</returns>
        [ServiceAuthorize]
        public async Task<ActionResult> MapScores(long id)
        {
            try
            {
                await Tracer.Info(string.Format("Refreshing scores of map with id '{0}'", id));

                var proxy = new AdministrationServiceClient(AuthenticationHelper.GetSessionId());
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