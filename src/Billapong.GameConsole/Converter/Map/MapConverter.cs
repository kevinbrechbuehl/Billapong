namespace Billapong.GameConsole.Converter.Map
{
    using Models;

    public static class MapConverter
    {
        public static Map ToEntity(Contract.Data.Map.Map contractMap)
        {
            var map = new Map {Id = contractMap.Id, Name = contractMap.Name};
            foreach (var contractWindow in contractMap.Windows)
            {
                map.Windows.Add(ToEntity(contractWindow));
            }

            return map;
        }

        public static Window ToEntity(Contract.Data.Map.Window contractWindow)
        {
            var window = new Window {Id = contractWindow.Id, X = contractWindow.X, Y = contractWindow.Y};

            foreach (var contractHole in contractWindow.Holes)
            {
                window.Holes.Add(ToEntity(contractHole));
            }

            return window;
        }

        public static Hole ToEntity(Contract.Data.Map.Hole contractHole)
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
