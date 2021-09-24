using Core.DomainModel.WeatherMapAggregate.Entities;
using Refit;
using System.Threading.Tasks;

namespace Infra.ApplicationServices.Utility.Http.OpenWeatherMap.Abstractions
{
    public interface IOpenWeatherMapApi
    {
        [Post("/weather?lat={lat}&lon={lon}&appid={apiKey}")]
        Task<WeatherMap> GetWeatherMap(float lat, float lon, string apiKey);
    }
}
