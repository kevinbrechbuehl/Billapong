namespace Billapong.MapEditor.Converter
{
    using Models;

    public static class MapConverter
    {
        public static Map ToEntity(this Contract.Data.Map.Map source)
        {
            return new Map
            {
                Name = source.Name
            };
        }
    }
}
