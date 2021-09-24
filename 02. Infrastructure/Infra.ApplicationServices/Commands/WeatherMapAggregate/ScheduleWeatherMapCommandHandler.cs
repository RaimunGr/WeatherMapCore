using Core.DomainModel.WeatherMapLogAggregate.Data;
using Core.DomainModel.WeatherMapLogAggregate.Entities;
using Hangfire;
using Infra.ApplicationServices.Utility.Http.OpenWeatherMap.Abstractions;
using Infra.ApplicationServices.Utility.MessageQueuing.Abstractions;
using Infra.Dal;
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
        private readonly IWeatherMapLogRepository _weatherMapLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessagePusher _messagePusher;

        public ScheduleWeatherMapCommandHandler(
            IOpenWeatherMapService openWeatherMapService,
            IWeatherMapLogRepository weatherMapLogRepository,
            IUnitOfWork unitOfWork,
            IMessagePusher messagePusher
        )
        {
            _openWeatherMapService = openWeatherMapService;
            _weatherMapLogRepository = weatherMapLogRepository;
            _unitOfWork = unitOfWork;
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
            var messageExp = FindMessageExp(rq);

            var rs = await _openWeatherMapService.GetWeatherMap(rq.Lat, rq.Lon);

            var fourteenCelsius = 14 + 273.15;
            if (rs.Main.Temp >= fourteenCelsius)
            {
                var log = WeatherMapLog.Create(rs.Main.Temp, rs.Name);
                _weatherMapLogRepository.Create(log);
                _unitOfWork.Commit();
            }

            PushMessage(messageExp, rs);
        }

        private void PushMessage(string messageExp, Core.DomainModel.WeatherMapAggregate.Entities.WeatherMap rs)
        {
            var queue = "WeatherMap_Queue";
            var exchange = "WeatherMap_Exchange";
            var routingKey = "WeatherMap_*";

            _messagePusher.DeclareQueue(queue);
            _messagePusher.DeclareExchange(exchange, ExchangeType.Direct);
            _messagePusher.BindQueue(queue, exchange, routingKey);
            _messagePusher.Push(exchange, routingKey, rs, messageExp);
        }

        private static string FindMessageExp(ScheduleWeatherMapCommand rq)
        {
            var schedule = CrontabSchedule.Parse(rq.Cron);
            var now = DateTime.UtcNow;
            var next = schedule.GetNextOccurrence(now);
            var messageExp = Math.Floor((next - now).TotalMilliseconds).ToString();
            return messageExp;
        }
    }
}
