namespace Billapong.MapEditor.Converter
{
    using System.Linq;
    using Models;

    /// <summary>
    /// Map object converter
    /// </summary>
    public static class MapConverter
    {
        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Map entity instance</returns>
        public static Map ToEntity(this Contract.Data.Map.Map source)
        {
            return new Map
            {
                Id = source.Id,
                Name = source.Name,
                IsPlayable = source.IsPlayable,
                Windows = source.Windows.ToList(),
                NumberOfWindows = source.Windows.Count,
                NumberOfHoles = source.Windows.Sum(window => window.Holes.Count)
            };
        }

        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="diameter">The diameter.</param>
        /// <returns>Hole entit instance</returns>
        public static Hole ToEntity(this Contract.Data.Map.Hole source, double diameter)
        {
            return new Hole
            {
                Id = source.Id,
                X = source.X,
                Y = source.Y,
                Diameter = diameter
            };
        }
    }
}
