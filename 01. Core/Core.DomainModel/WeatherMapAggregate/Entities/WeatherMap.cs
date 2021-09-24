using System.Collections.Generic;

namespace Core.DomainModel.WeatherMapAggregate.Entities
{
    public sealed class WeatherMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Coord Coord { get; set; }
        public IEnumerable<Weather> Weather { get; set; }
        public string Base { get; set; }
        public MainInfo Main { get; set; }
        public int Visibility { get; set; }
        public WindInfo Wind { get; set; }
        public Clouds Clouds { get; set; }
        public int Dt { get; set; }
        public SysInfo Sys { get; set; }
        public int Timezone { get; set; }
        public int Cod { get; set; }
    }
}
