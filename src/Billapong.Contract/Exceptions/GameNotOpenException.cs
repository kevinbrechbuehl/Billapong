namespace Billapong.Contract.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = "GameNotOpenException", Namespace = Globals.ExceptionContractNamespaceName)]
    public class GameNotOpenException
    {
        [DataMember(Name = "GameId", Order = 1)]
        public Guid GameId { get; set; }

        [DataMember(Name = "Message", Order = 1)]
        public string Message { get; set; }

        public GameNotOpenException(Guid gameId)
        {
            this.GameId = gameId;
            this.Message = string.Format("The game with id '{0}' is not in opening state", gameId);
        }
    }
}
