namespace Billapong.MapEditor.Models
{
    using System.Collections.Generic;
    using Core.Client.UI;

    /// <summary>
    /// Map model.
    /// </summary>
    public class Map : NotificationObject
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.GetValue<string>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current map is playable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if map is playable; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlayable
        {
            get
            {
                return this.GetValue<bool>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the number of windows.
        /// </summary>
        /// <value>
        /// The number of windows.
        /// </value>
        public int NumberOfWindows
        {
            get
            {
                return this.GetValue<int>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the number of holes.
        /// </summary>
        /// <value>
        /// The number of holes.
        /// </value>
        public int NumberOfHoles
        {
            get
            {
                return this.GetValue<int>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the windows.
        /// </summary>
        /// <value>
        /// The windows.
        /// </value>
        public IList<Contract.Data.Map.Window> Windows { get; set; }
    }
}
