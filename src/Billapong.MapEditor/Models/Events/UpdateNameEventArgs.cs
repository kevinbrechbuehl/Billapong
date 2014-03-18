namespace Billapong.MapEditor.Models.Events
{
    using System;

    /// <summary>
    /// Event arguments for updating the name.
    /// </summary>
    public class UpdateNameEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNameEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public UpdateNameEventArgs(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }
    }
}
