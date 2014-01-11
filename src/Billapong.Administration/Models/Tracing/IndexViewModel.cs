namespace Billapong.Administration.Models.Tracing
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// View model for the index view of the tracing controller.
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// Gets or sets the log level identifier.
        /// </summary>
        /// <value>
        /// The log level identifier.
        /// </value>
        [Display(Name = "LogLevelLabel", ResourceType = typeof(Resources.Global))]
        public int LogLevelId { get; set; }

        /// <summary>
        /// Gets or sets the log level list.
        /// </summary>
        /// <value>
        /// The log level list.
        /// </value>
        public IEnumerable<SelectListItem> LogLevelList { get; set; }

        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        /// <value>
        /// The component identifier.
        /// </value>
        [Display(Name = "ComponentLabel", ResourceType = typeof(Resources.Global))]
        public string ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the component list.
        /// </summary>
        /// <value>
        /// The component list.
        /// </value>
        public IEnumerable<SelectListItem> ComponentList { get; set; }

        /// <summary>
        /// Gets or sets the number of entries identifier.
        /// </summary>
        /// <value>
        /// The number of entries identifier.
        /// </value>
        [Display(Name = "NumberOfEntriesLabel", ResourceType = typeof(Resources.Global))]
        public int NumberOfEntriesId { get; set; }

        /// <summary>
        /// Gets or sets the number of entries list.
        /// </summary>
        /// <value>
        /// The number of entries list.
        /// </value>
        public IEnumerable<SelectListItem> NumberOfEntriesList { get; set; }
    }
}