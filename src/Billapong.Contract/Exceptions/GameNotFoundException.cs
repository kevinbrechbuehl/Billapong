namespace Billapong.Contract.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = "GameNotFoundException", Namespace = Globals.ExceptionContractNamespaceName)]
    public class GameNotFoundException
    {
        [DataMember(Name = "GameId", Order = 1)]
        public Guid GameId { get; set; }

        [DataMember(Name = "Message", Order = 1)]
        public string Message { get; set; }

        public GameNotFoundException(Guid gameId)
        {
            this.GameId = gameId;
            this.Message = string.Format("The game with id '{0}' was not found in the lobby", gameId);
        }
    }
}
