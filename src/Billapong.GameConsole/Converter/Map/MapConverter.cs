namespace Billapong.GameConsole.Converter.Map
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    /// <summary>
    /// Provides methods to convert between map entities and contracts
    /// </summary>
    public static class MapConverter
    {
        /// <summary>
        /// Converts the map contract to the corresponding entity
        /// </summary>
        /// <param name="contractMap">The contract map.</param>
        /// <returns>
        /// The entity
        /// </returns>
        public static Map ToEntity(this Contract.Data.Map.Map contractMap)
        {
           return ToEntity(contractMap, Enumerable.Empty<long>());
        }

        /// <summary>
        /// Converts the map contract to the corresponding entity
        /// </summary>
        /// <param name="contractMap">The contract map.</param>
        /// <param name="visibleWindows">The visible windows.</param>
        /// <returns>
        /// The entity
        /// </returns>
        public static Map ToEntity(this Contract.Data.Map.Map contractMap, IEnumerable<long> visibleWindows)
        {
            var map = new Map { Id = contractMap.Id, Name = contractMap.Name };
            var ownedWindows = visibleWindows as long[] ?? visibleWindows.ToArray();

            foreach (var contractWindow in contractMap.Windows)
            {
                var entityWindow = ToEntity(contractWindow);
                entityWindow.IsOwnWindow = ownedWindows.Contains(entityWindow.Id);
                map.Windows.Add(entityWindow);
            }

            return map;
        }

        /// <summary>
        /// Converts the Window contract to the corresponding entity
        /// </summary>
        /// <param name="contractWindow">The contract window.</param>
        /// <returns>The entity</returns>
        public static Window ToEntity(this Contract.Data.Map.Window contractWindow)
        {
            var window = new Window { Id = contractWindow.Id, X = contractWindow.X, Y = contractWindow.Y };

            foreach (var contractHole in contractWindow.Holes)
            {
                window.Holes.Add(ToEntity(contractHole));
            }

            return window;
        }

        /// <summary>
        /// Converts the Hole contract to the corresponding entity
        /// </summary>
        /// <param name="contractHole">The contract hole.</param>
        /// <returns>The entity</returns>
        public static Hole ToEntity(this Contract.Data.Map.Hole contractHole)
        {
            return new Hole()
            {
                Id = contractHole.Id,
                X = contractHole.X,
                Y = contractHole.Y
            };
        }
    }
}
