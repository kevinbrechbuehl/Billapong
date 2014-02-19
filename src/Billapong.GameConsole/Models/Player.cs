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
        /// The round state
        /// </summary>
        private RoundState roundState;

        private SolidColorBrush colorBrush;

        /// <summary>
        /// The different round states for the player
        /// </summary>
        public enum RoundState
        {
            /// <summary>
            /// The current turn belongs to the opponent
            /// </summary>
            /// 
            OpponentsTurn,

            /// <summary>
            /// The players ball is placed
            /// </summary>
            BallPlaced,

            /// <summary>
            /// The ball of the current player is moving
            /// </summary>
            BallMoving
        }

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
        /// Gets or sets the color brush.
        /// </summary>
        /// <value>
        /// The color brush.
        /// </value>
        public SolidColorBrush ColorBrush
        {
            get
            {
                if (this.colorBrush == null)
                {
                    this.ColorBrush = new SolidColorBrush(this.PlayerColor);    
                }

                return this.colorBrush;
            }

            private set
            {
                this.colorBrush = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the state of the current round.
        /// </summary>
        /// <value>
        /// The state of the current round.
        /// </value>
        public RoundState CurrentRoundState
        {
            get
            {
                return this.roundState;
            }

            set
            {
                this.roundState = value;
                OnPropertyChanged();
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
