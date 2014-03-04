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
        private PlayerState playerState;

        /// <summary>
        /// The color brush
        /// </summary>
        private SolidColorBrush colorBrush;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isFirstPlayer">if set to <c>true</c> this player started the game.</param>
        /// <param name="isLocalPlayer">if set to <c>true</c> the player represents the local user.</param>
        /// <param name="hasFirstTurn">if set to <c>true</c> [has first turn].</param>
        public Player(string name, bool isFirstPlayer, bool isLocalPlayer, bool hasFirstTurn)
        {
            this.Name = name;
            this.IsFirstPlayer = isFirstPlayer;
            this.IsLocalPlayer = isLocalPlayer;
            this.HasFirstTurn = hasFirstTurn;
        }

        /// <summary>
        /// The different round states for the player
        /// </summary>
        public enum PlayerState
        {
            /// <summary>
            /// The current turn belongs to the opponent
            /// </summary>
            OpponentsTurn,

            /// <summary>
            /// The players ball is placed
            /// </summary>
            BallPlaced,

            /// <summary>
            /// The ball of the current player is moving
            /// </summary>
            BallMoving,

            /// <summary>
            /// The player won the game
            /// </summary>
            Won,

            /// <summary>
            /// The player lost the game
            /// </summary>
            Lost,

            /// <summary>
            /// The player played a draw
            /// </summary>
            Draw
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
        /// Gets a value indicating whether the player has the first turn.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the player has the first turn; otherwise, <c>false</c>.
        /// </value>
        public bool HasFirstTurn { get; private set; }

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
        /// Gets the color brush.
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
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the state of the current player.
        /// </summary>
        /// <value>
        /// The state of the current game.
        /// </value>
        public PlayerState CurrentPlayerState
        {
            get
            {
                return this.playerState;
            }

            set
            {
                this.playerState = value;
                this.OnPropertyChanged();
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
            get
            {
                return this.score; 
            }

            set
            {
                this.score = value;
                this.OnPropertyChanged();
            }
        }
    }
}
