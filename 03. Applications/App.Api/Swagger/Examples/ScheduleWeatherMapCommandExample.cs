using Infra.ApplicationServices.Commands.WeatherMapAggregate;
using Swashbuckle.AspNetCore.Filters;

namespace App.Api.Swagger.Examples
{
    public sealed class ScheduleWeatherMapCommandExample : IExamplesProvider<ScheduleWeatherMapCommand>
    {
        public ScheduleWeatherMapCommand GetExamples()
        {
            return new ScheduleWeatherMapCommand
            {
                Lat = 51.5074f,
                Lon = -0.1278f,
                Cron = "*/5 * * * *",
            };
        }
    }
}
