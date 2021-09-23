using Core.DomainModel.WeatherMapAggregate;
using System.Threading.Tasks;

namespace Infra.ApplicationServices.Utility.Http.OpenWeatherMap.Abstractions
{
    public interface IOpenWeatherMapService
    {
        Task<WeatherMap> GetWeatherMap(float lat, float lon);
    }
}
