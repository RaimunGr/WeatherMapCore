using Hangfire;
using Infra.ApplicationServices.Utility.Http.OpenWeatherMap.Abstractions;
using Infra.ApplicationServices.Utility.MessageQueuing.Abstractions;
using MediatR;
using NCrontab;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.ApplicationServices.Commands.WeatherMapAggregate
{
    public sealed class ScheduleWeatherMapCommandHandler
        : IRequestHandler<ScheduleWeatherMapCommand, string>
    {
        private readonly IOpenWeatherMapService _openWeatherMapService;
        private readonly IMessagePusher _messagePusher;

        public ScheduleWeatherMapCommandHandler(
            IOpenWeatherMapService openWeatherMapService,
            IMessagePusher messagePusher
        )
        {
            _openWeatherMapService = openWeatherMapService;
            _messagePusher = messagePusher;
        }

        public async Task<string> Handle(
            ScheduleWeatherMapCommand rq,
            CancellationToken cancellationToken
        )
        {
            await ManageWeatherMap(rq);

            RecurringJob.AddOrUpdate(
                $"WeatherMapJob_{rq.Lat}_{rq.Lon}",
                () => ManageWeatherMap(rq),
                rq.Cron
            );

            return "Weather map scheduled for the given cron.";
        }

        public async Task ManageWeatherMap(ScheduleWeatherMapCommand rq)
        {
            var schedule = CrontabSchedule.Parse(rq.Cron);
            var now = DateTime.UtcNow;
            var next = schedule.GetNextOccurrence(now);
            var messageExp = Math.Floor((next - now).TotalMilliseconds).ToString();

            var rs = await _openWeatherMapService.GetWeatherMap(rq.Lat, rq.Lon);
            var queue = "WeatherMap_Queue";
            var exchange = "WeatherMap_Exchange";
            var routingKey = "WeatherMap_*";

            _messagePusher.DeclareQueue(queue);
            _messagePusher.DeclareExchange(exchange, ExchangeType.Direct);
            _messagePusher.BindQueue(queue, exchange, routingKey);
            _messagePusher.Push(exchange, routingKey, rs, messageExp);
        }
    }
}
