namespace Billapong.Core.Server.Converter.Editor
{
    using System.Linq;

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
        public static DataAccess.Model.Editor.Map ToEntity(this Contract.Data.Editor.Map source)
        {
            return new DataAccess.Model.Editor.Map
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
        public static Contract.Data.Editor.Map ToContract(this DataAccess.Model.Editor.Map source)
        {
            return new Contract.Data.Editor.Map
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
        private static DataAccess.Model.Editor.Window ToEntity(this Contract.Data.Editor.Window source)
        {
            return new DataAccess.Model.Editor.Window
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
        private static Contract.Data.Editor.Window ToContract(this DataAccess.Model.Editor.Window source)
        {
            return new Contract.Data.Editor.Window
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
        private static DataAccess.Model.Editor.Hole ToEntity(this Contract.Data.Editor.Hole source)
        {
            return new DataAccess.Model.Editor.Hole
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
        private static Contract.Data.Editor.Hole ToContract(this DataAccess.Model.Editor.Hole source)
        {
            return new Contract.Data.Editor.Hole
            {
                Id = source.Id,
                X = source.X,
                Y = source.Y
            };
        }
    }
}
