using Core.DomainModel.WeatherMapLogAggregate.Data;
using Infra.Dal.UserAggregate;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Dal
{
    public static class DataAccessLayerBootstrapper
    {
        public static void AddDataAccessLayer(this IServiceCollection services, string connectionString)
        {
            services.AddMemoryCache();
            services.AddDbContext<AppDbContext>(
                opt => opt.UseSqlServer(connectionString)
            );

            services.AddScoped<IWeatherMapLogRepository, WeatherMapLogRepository>();
            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped<IUnitOfWork, AppUnitOfWork>();
        }

        public static void UseDatabase(this IApplicationBuilder app)
        {
            try
            {
                using var scope = app.ApplicationServices.CreateScope();
                using var context = scope.ServiceProvider.GetService<AppDbContext>();
                context.Database.Migrate();
            }
            catch
            { }
        }
    }
}
