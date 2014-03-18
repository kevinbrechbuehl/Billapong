namespace Billapong.MapEditor.Models.Events
{
    using System;

    /// <summary>
    /// Event arguments for update is playable flag.
    /// </summary>
    public class UpdateIsPlayableEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateIsPlayableEventArgs"/> class.
        /// </summary>
        /// <param name="isPlayable">if set to <c>true</c> the map is current playable.</param>
        public UpdateIsPlayableEventArgs(bool isPlayable)
        {
            this.IsPlayable = isPlayable;
        }

        /// <summary>
        /// Gets a value indicating whether the map is playable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if map is playable; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlayable { get; private set; }
    }
}
