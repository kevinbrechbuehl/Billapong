namespace Billapong.GameConsole.Models
{
    using System.Windows.Media;
    using Core.Client.UI;

    /// <summary>
    /// Represents a player
    /// </summary>
    public class Player : NotificationObject
    {
        /// <summary>
        /// The score
        /// </summary>
        private int score;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isFirstPlayer">if set to <c>true</c> [is first player].</param>
        /// <param name="isLocalPlayer">if set to <c>true</c> [is local player].</param>
        public Player(string name, bool isFirstPlayer, bool isLocalPlayer)
        {
            this.Name = name;
            this.IsFirstPlayer = isFirstPlayer;
            this.IsLocalPlayer = isLocalPlayer;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the player is the first player.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the player is the first player; otherwise, <c>false</c>.
        /// </value>
        public bool IsFirstPlayer { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the player is the local player.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the player is the local player; otherwise, <c>false</c>.
        /// </value>
        public bool IsLocalPlayer { get; private set; }

        /// <summary>
        /// Gets the color of the player.
        /// </summary>
        /// <value>
        /// The color of the player.
        /// </value>
        public Color PlayerColor
        {
            get
            {
                return this.IsFirstPlayer ? Colors.Red : Colors.Blue;
            }
        }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                OnPropertyChanged();
            }
        }
    }
}
