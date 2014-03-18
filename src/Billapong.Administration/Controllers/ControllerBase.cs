namespace Billapong.Administration.Controllers
{
    using System;
    using System.Web.Mvc;
    using Billapong.Core.Client.Tracing;

    /// <summary>
    /// Abstract base class for all MVC controllers.
    /// </summary>
    public abstract class ControllerBase : Controller
    {
        /// <summary>
        /// Handles an exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception.</param>
        protected async void HandleException(string message, Exception ex)
        {
            await Tracer.Error(message, ex);
        }
    }
}