using Core.DomainModel.WeatherMapLogAggregate.Entities;
using System.Collections.Generic;

namespace Core.DomainModel.WeatherMapLogAggregate.Data
{
    public interface IWeatherMapLogRepository
    {
        WeatherMapLog Create(WeatherMapLog log);
        IEnumerable<WeatherMapLog> GetAll();
    }
}
