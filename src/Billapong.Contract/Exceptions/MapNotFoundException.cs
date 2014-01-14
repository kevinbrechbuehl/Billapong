namespace Billapong.Contract.Exceptions
{
    using System;

    public class MapNotFoundException : Exception
    {
        public MapNotFoundException()
        {
        }

        public MapNotFoundException(long mapId)
            : base(string.Format("Map with id '{0}' not found.", mapId))
        {
        }
    }
}
