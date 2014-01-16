namespace Billapong.Contract.Exceptions
{
    using System.Runtime.Serialization;

    [DataContract(Name = "MapNotFoundException", Namespace = Globals.ExceptionContractNamespaceName)]
    public class MapNotFoundException
    {
        [DataMember(Name = "MapId", Order = 1)]
        public long MapId { get; set; }

        [DataMember(Name = "Message", Order = 1)]
        public string Message { get; set; }

        public MapNotFoundException(long mapId)
        {
            this.MapId = mapId;
            this.Message = string.Format("Map with id '{0}' not found.", mapId);
        }
    }
}
