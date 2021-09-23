using Core.DomainModel.WeatherMapAggregate;
using MediatR;

namespace Infra.ApplicationServices.Commands.WeatherMapAggregate
{
    public sealed class ScheduleWeatherMapCommand : IRequest<string>
    {
        public float Lat { get; set; }
        public float Lon { get; set; }
        public string Cron { get; set; }
    }
}
