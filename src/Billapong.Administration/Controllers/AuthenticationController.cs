using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Billapong.Administration.Controllers
{
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Billapong.Administration.Authorization;
    using Billapong.Administration.Models.Authentication;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Exceptions;
    using Billapong.Core.Client.Authentication;
    using Billapong.Core.Client.Tracing;

    public class AuthenticationController : ControllerBase
    {
        public ActionResult Login()
        {
            return this.View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.HasErrors = true;
                return this.View(model);
            }

            try
            {
                await Tracer.Info("Trying to login");

                var proxy = new AuthenticationServiceClient();
                var sessionId = proxy.Login(model.Username, model.Password, Role.Administrator);
                AuthenticationHelper.SessionId = sessionId;

                var returnUrl = Request.QueryString["ReturnUrl"];
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return this.Redirect(returnUrl);
                }

                return this.RedirectToAction("Index", "Home");
            }
            catch (FaultException<LoginFailedException> ex)
            {
                ModelState.AddModelError(string.Empty, Resources.Global.LoginFailed);
                model.HasErrors = true;
                this.HandleException("Login failed", ex);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, Resources.Global.ErrorOccurredInline);
                model.HasErrors = true;
                this.HandleException("Error while trying to login", ex);
            }

            return this.View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationHelper.SessionId = null;
            return this.RedirectToAction("Index", "Home");
        }
	}
}