using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Billapong.Administration.Controllers
{
    using System.Net;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Billapong.Administration.Authorization;
    using Billapong.Contract.Exceptions;
    using Core.Client.Tracing;
    using Service;

    public class GameController : ControllerBase
    {
        [ServiceAuthorize]
        public ActionResult Index()
        {
            return View();
        }

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