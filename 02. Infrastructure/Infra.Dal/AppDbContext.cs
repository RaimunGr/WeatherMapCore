using Core.DomainModel.WeatherMapLogAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infra.Dal
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<WeatherMapLog> WeatherMapLogs { get; set; }
    }
}