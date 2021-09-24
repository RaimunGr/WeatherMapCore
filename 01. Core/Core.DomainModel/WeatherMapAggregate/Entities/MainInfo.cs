using System.Text.Json.Serialization;

namespace Core.DomainModel.WeatherMapAggregate.Entities
{
    public sealed class MainInfo
    {
        public float Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public float FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public float TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public float TempMax { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }
}
