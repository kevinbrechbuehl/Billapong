namespace Billapong.Administration.Controllers
{
    using System;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Billapong.Administration.Authorization;
    using Billapong.Administration.Models.Authentication;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Exceptions;
    using Billapong.Core.Client.Authentication;
    using Billapong.Core.Client.Tracing;

    /// <summary>
    /// MVC controller for authentication (login/logout).
    /// </summary>
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// Login action.
        /// </summary>
        /// <returns>The view.</returns>
        public ActionResult Login()
        {
            return this.View(new LoginViewModel());
        }

        /// <summary>
        /// Login action for http post.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The view if login failed, redirect action if login was ok</returns>
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

        /// <summary>
        /// Logouts the user.
        /// </summary>
        /// <returns>Redirect to root page</returns>
        public ActionResult Logout()
        {
            AuthenticationHelper.SessionId = null;
            return this.RedirectToAction("Index", "Home");
        }
    }
}