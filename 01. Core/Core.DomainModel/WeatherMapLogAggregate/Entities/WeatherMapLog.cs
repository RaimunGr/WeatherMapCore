using System;

namespace Core.DomainModel.WeatherMapLogAggregate.Entities
{
    public sealed class WeatherMapLog
    {
        private WeatherMapLog()
        { }

        public Guid Id { get; private set; }
        public float Temp { get; private set; }
        public string City { get; private set; }
        public DateTime CreationDate { get; private set; }

        public static WeatherMapLog Create(float temp, string city)
        {
            return new WeatherMapLog
            {
                Id = Guid.NewGuid(),
                Temp = temp,
                City = city,
                CreationDate = DateTime.UtcNow,
            };
        }
    }
}
