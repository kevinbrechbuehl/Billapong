namespace Billapong.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Contract.Data.Tracing;
    using Core.Client.Tracing;
    using Models.Tracing;

    /// <summary>
    /// The tracing controller
    /// </summary>
    public class TracingController : Controller
    {
        /// <summary>
        /// The index action which shows the tracing entries from the server's database.
        /// </summary>
        /// <returns>The result view.</returns>
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            // add loglevels
            var logLevels = Enum.GetValues(typeof (LogLevel)).Cast<LogLevel>();
            model.LogLevelList = logLevels.Select(level => new SelectListItem { Text = level.ToString(), Value = ((int)level).ToString() });

            // add components
            var components = Enum.GetValues(typeof (Component)).Cast<Component>();
            model.ComponentList = components.Select(component => new SelectListItem { Text = component.ToString(), Value = component.ToString() });

            // add number of entries
            // this is not that nice, but for easier usage it's ok for now :)
            var numberOfEntries = new List<SelectListItem>
            {
                new SelectListItem { Text = Resources.Global.All, Value = "0" },
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "100", Value = "100" },
                new SelectListItem { Text = "1000", Value = "1000" },
            };

            model.NumberOfEntriesList = numberOfEntries;
            model.NumberOfEntriesId = 100;

            return this.View(model);
        }

        /// <summary>
        /// Get log entries over the WCF service
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="numberOfEntries">The number of entries.</param>
        /// <returns>Partial view with a table containing all requested log entries</returns>
        public ActionResult Entries(Component component = Component.All, LogLevel logLevel = LogLevel.Debug, int numberOfEntries = 0)
        {
            return this.PartialView(Tracer.GetLogMessages(component, logLevel, numberOfEntries));
        }
    }
}