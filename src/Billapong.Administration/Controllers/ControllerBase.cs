using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billapong.Administration.Controllers
{
    using System.Web.Mvc;
    using Billapong.Core.Client.Tracing;

    public abstract class ControllerBase : Controller
    {
        protected async void HandleException(string message, Exception ex)
        {
            await Tracer.Error(message, ex);
        }
    }
}