using Infra.ApplicationServices.Commands.WeatherMapAggregate;
using Infra.ApplicationServices.Utility.Http.Authentication.Abstractions;
using Infra.ApplicationServices.Utility.Http.Authentication.Implementations;
using Infra.ApplicationServices.Utility.Http.OpenWeatherMap.Abstractions;
using Infra.ApplicationServices.Utility.Http.OpenWeatherMap.Implementations;
using Infra.ApplicationServices.Utility.MessageQueuing.Abstractions;
using Infra.ApplicationServices.Utility.MessageQueuing.Implementations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.ApplicationServices
{
    public static class ApplicationServicesBootstrapper
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ScheduleWeatherMapCommand).Assembly);
        }

        public static void AddAuthenticationApi(this IServiceCollection services, string baseAddress)
        {
            services.AddScoped<IAuthService, AuthService>(p =>
            {
                return new AuthService(baseAddress);
            });
        }

        public static void AddOpenWeatherMapApi(this IServiceCollection services, string baseAddress, string apiKey)
        {
            services.AddScoped<IOpenWeatherMapService, OpenWeatherMapService>(p =>
            {
                return new OpenWeatherMapService(baseAddress, apiKey);
            });
        }

        public static void AddMessageQueuing(this IServiceCollection services, string amqpUri)
        {
            services.AddSingleton<IMessagePusher, MessagePusher>(
                provider => new MessagePusher(amqpUri)
            );
        }
    }
}
