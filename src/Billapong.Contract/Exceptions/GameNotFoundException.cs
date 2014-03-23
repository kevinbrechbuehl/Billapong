namespace Billapong.Contract.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception for invalid game.
    /// </summary>
    [DataContract(Name = "GameNotFoundException", Namespace = Globals.ExceptionContractNamespaceName)]
    public class GameNotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameNotFoundException"/> class.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        public GameNotFoundException(Guid gameId)
        {
            this.GameId = gameId;
            this.Message = string.Format("The game with id '{0}' was not found in the lobby", gameId);
        }

        /// <summary>
        /// Gets or sets the game identifier.
        /// </summary>
        /// <value>
        /// The game identifier.
        /// </value>
        [DataMember(Name = "GameId", Order = 1)]
        public Guid GameId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember(Name = "Message", Order = 1)]
        public string Message { get; set; }
    }
}
