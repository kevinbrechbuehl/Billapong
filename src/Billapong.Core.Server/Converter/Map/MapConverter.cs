namespace Billapong.Core.Server.Converter.Map
{
    using System.Linq;
    using DataAccess.Model.Map;

    /// <summary>
    /// Converter extensions for the map objet.
    /// </summary>
    public static class MapConverter
    {
        /// <summary>
        /// Convert the map contract to an entity object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The entity object</returns>
        public static Map ToEntity(this Contract.Data.Map.Map source)
        {
            return new Map
            {
                Id = source.Id,
                Name = source.Name,
                Windows = source.Windows.Select(window => window.ToEntity()).ToList()
            };
        }

        /// <summary>
        /// Convert the map entity to a contract object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The contract object</returns>
        public static Contract.Data.Map.Map ToContract(this Map source)
        {
            return new Contract.Data.Map.Map
            {
                Id = source.Id,
                Name = source.Name,
                Windows = source.Windows.Select(window => window.ToContract()).ToList()
            };
        }

        /// <summary>
        /// Convert the window contract to an entity object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The entity object</returns>
        private static Window ToEntity(this Contract.Data.Map.Window source)
        {
            return new Window
            {
                Id = source.Id,
                X = source.X,
                Y = source.Y,
                Holes = source.Holes.Select(hole => hole.ToEntity()).ToList()
            };
        }

        /// <summary>
        /// Convert the window entity to a contract object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The contract object</returns>
        private static Contract.Data.Map.Window ToContract(this Window source)
        {
            return new Contract.Data.Map.Window
            {
                Id = source.Id,
                X = source.X,
                Y = source.Y,
                Holes = source.Holes.Select(hole => hole.ToContract()).ToList()
            };
        }

        /// <summary>
        /// Convert the hole contract to an entity object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The entity object</returns>
        private static Hole ToEntity(this Contract.Data.Map.Hole source)
        {
            return new Hole
            {
                Id = source.Id,
                X = source.X,
                Y = source.Y
            };
        }

        /// <summary>
        /// Convert the hole entity to a contract object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The contract object</returns>
        private static Contract.Data.Map.Hole ToContract(this Hole source)
        {
            return new Contract.Data.Map.Hole
            {
                Id = source.Id,
                X = source.X,
                Y = source.Y
            };
        }
    }
}
