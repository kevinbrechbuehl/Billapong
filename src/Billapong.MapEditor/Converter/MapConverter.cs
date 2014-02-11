namespace Billapong.MapEditor.Converter
{
    using System.Linq;
    using Models;

    public static class MapConverter
    {
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

        public static Contract.Data.Map.GeneralMapData ToGeneralMapData(this Map source)
        {
            return new Contract.Data.Map.GeneralMapData
            {
                Id = source.Id,
                Name = source.Name
            };
        }
    }
}
