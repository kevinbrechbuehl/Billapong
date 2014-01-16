namespace Billapong.Contract.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class GameNotOpenException : Exception
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

        public GameNotOpenException(Guid gameId)
            : base(string.Format("The game with id '{0}' is not in opening state", gameId))
        {
            this.GameId = gameId;
        }
    }
}
