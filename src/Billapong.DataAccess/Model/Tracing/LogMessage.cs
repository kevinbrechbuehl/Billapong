namespace Billapong.DataAccess.Model.Tracing
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The log message domain model.
    /// </summary>
    public class LogMessage : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        [Required]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        [Required]
        public string LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        /// <value>
        /// The module.
        /// </value>
        [Required]
        public string Module { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [Required]
        public string Message { get; set; }
    }
}
