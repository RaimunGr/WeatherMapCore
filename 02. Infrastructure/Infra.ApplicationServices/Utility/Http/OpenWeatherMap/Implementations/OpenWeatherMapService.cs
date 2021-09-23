using Core.DomainModel.WeatherMapAggregate;
using Infra.ApplicationServices.Utility.Http.OpenWeatherMap.Abstractions;
using Refit;
using System.Threading.Tasks;

namespace Infra.ApplicationServices.Utility.Http.OpenWeatherMap.Implementations
{
    public sealed class OpenWeatherMapService : IOpenWeatherMapService
    {
        private readonly IOpenWeatherMapApi _api;
        private readonly string _apiKey;

        public OpenWeatherMapService(string baseAddress, string apiKey)
        {
            _api = RestService.For<IOpenWeatherMapApi>(baseAddress);
            _apiKey = apiKey;
        }

        public Task<WeatherMap> GetWeatherMap(float lat, float lon)
        {
            return _api.GetWeatherMap(lat, lon, _apiKey);
        }
    }
}
