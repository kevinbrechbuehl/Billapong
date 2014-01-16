namespace Billapong.Contract.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class GameNotFoundException : Exception
    {
        [DataMember]
        public Guid GameId { get; set; }

        [DataMember]
        public string ErrorMessage
        {
            get
            {
                return this.Message;
            }
        }

        public GameNotFoundException(Guid gameId)
            : base(string.Format("The game with id '{0}' was not found in the lobby", gameId))
        {
            this.GameId = gameId;
        }
    }
}
