namespace Core.DomainModel.WeatherMapAggregate
{
    public sealed class WindInfo
    {
        public float Speed { get; set; }
        public int Deg { get; set; }
        public float Gust { get; set; }
    }
}
