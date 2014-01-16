namespace Billapong.Contract.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class MapNotFoundException : Exception
    {
        [DataMember]
        public long MapId { get; set; }

        [DataMember]
        public string ErrorMessage
        {
            get
            {
                return this.Message;
            }
        }

        public MapNotFoundException(long mapId)
            : base(string.Format("Map with id '{0}' not found.", mapId))
        {
            this.MapId = mapId;
        }
    }
}
