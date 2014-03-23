namespace Billapong.Core.Server.Utilities
{
    using System.Collections.Generic;
    using System.Linq;
    using Billapong.DataAccess.Model.Map;

    /// <summary>
    /// Helper methods for maps.
    /// </summary>
    public static class MapUtil
    {
        /// <summary>
        /// Determines whether the specified map is playable.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>True if map is playable, false otherwise</returns>
        public static bool IsPlayable(Map map)
        {
            if (map.Windows.Count == 0) return false;
            if (map.Windows.Sum(window => window.Holes.Count) == 0) return false;
            if (map.Windows.Count == 1) return true;

            var graph = GetActiveGraph(map, new List<long>(), map.Windows.First());
            return map.Windows.All(window => graph.Contains(window.Id));
        }

        /// <summary>
        /// Gets the graph with all active windows.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="window">The window.</param>
        /// <returns>List with all window id's based on the graph</returns>
        private static IList<long> GetActiveGraph(Map map, IList<long> graph, Window window)
        {
            graph.Add(window.Id);

            // neighbor to the right is active
            var sibling = map.Windows.FirstOrDefault(neighbor => neighbor.X == window.X + 1 && neighbor.Y == window.Y);
            if (sibling != null && !graph.Contains(sibling.Id))
            {
                graph = GetActiveGraph(map, graph, sibling);
            }

            // neighbor to the left is active
            sibling = map.Windows.FirstOrDefault(neighbor => neighbor.X == window.X - 1 && neighbor.Y == window.Y);
            if (sibling != null && !graph.Contains(sibling.Id))
            {
                graph = GetActiveGraph(map, graph, sibling);
            }

            // neighbor below is active
            sibling = map.Windows.FirstOrDefault(neighbor => neighbor.X == window.X && neighbor.Y == window.Y + 1);
            if (sibling != null && !graph.Contains(sibling.Id))
            {
                graph = GetActiveGraph(map, graph, sibling);
            }

            // neightbor above is active
            sibling = map.Windows.FirstOrDefault(neighbor => neighbor.X == window.X && neighbor.Y == window.Y - 1);
            if (sibling != null && !graph.Contains(sibling.Id))
            {
                graph = GetActiveGraph(map, graph, sibling);
            }

            return graph;
        }
    }
}
