﻿namespace Billapong.Administration.Controllers
{
    using System.Web.Mvc;

    using Billapong.Core.Client.Tracing;

    /// <summary>
    /// The home controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The index action
        /// </summary>
        /// <returns>The view</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}