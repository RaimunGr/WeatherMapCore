using Core.DomainModel.WeatherMapLogAggregate.Data;
using Core.DomainModel.WeatherMapLogAggregate.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infra.Dal.UserAggregate
{
    public sealed class WeatherMapLogRepository : IWeatherMapLogRepository
    {
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _dbContext;
        const string CACHE_NAME = "users";

        public WeatherMapLogRepository(IMemoryCache cache, AppDbContext dbContext)
        {
            _cache = cache;
            _dbContext = dbContext;
        }

        public WeatherMapLog Create(WeatherMapLog log)
        {
            if (log is null)
            {
                throw new ArgumentNullException("Log can NOT be null");
            }

            var entry = _dbContext.WeatherMapLogs.Add(log);

            return entry.Entity;
        }

        public IEnumerable<WeatherMapLog> GetAll()
        {
            return _dbContext.WeatherMapLogs.ToList();
        }
    }
}
