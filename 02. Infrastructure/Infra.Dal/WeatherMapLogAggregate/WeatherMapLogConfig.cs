using Core.DomainModel.WeatherMapLogAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Dal.UserAggregate
{
    public sealed class WeatherMapLogConfig : IEntityTypeConfiguration<WeatherMapLog>
    {
        public void Configure(EntityTypeBuilder<WeatherMapLog> builder)
        {
            builder.HasKey(u => u.Id);

            builder
                .Property(b => b.Temp)
                .IsRequired();

            builder
                .Property(b => b.City)
                .IsRequired();
        }
    }
}
