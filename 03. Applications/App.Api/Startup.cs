using App.Api.Attributes;
using App.Api.Swagger.Examples;
using App.Api.Utility;
using Hangfire;
using Hangfire.SqlServer;
using Infra.ApplicationServices;
using Infra.ApplicationServices.Utility.Http.Authentication.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Text.Json.Serialization;

namespace App.Api.Admin.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices();
            services.AddMessageQueuing(Configuration.GetValue<string>("MessageQueuing:AmqpUri"));
            services.AddOpenWeatherMapApi(
                Configuration.GetValue<string>("OpenWeatherMap:BaseAddress"),
                Configuration.GetValue<string>("OpenWeatherMap:ApiKey")
            );
            services.AddAuthenticationApi(Configuration.GetValue<string>("Auth:BaseAddress"));

            services.AddControllers(opt => opt.Filters.Add<AuthenticationExceptionFilterAttribute>())
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Raimun - Weather Map API",
                    Version = "v1"
                });
                c.ExampleFilters();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                        },
                        Array.Empty<string>()
                    }
                });

                var documentPath = Path.Combine(Directory.GetCurrentDirectory(), "App.Api.xml");
                c.IncludeXmlComments(documentPath);
            });

            services.AddSwaggerExamplesFromAssemblyOf<ScheduleWeatherMapCommandExample>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SecurityTokenValidators.Clear();
                var baseAddress = Configuration.GetValue<string>("Auth:BaseAddress");
                x.SecurityTokenValidators.Add(new RaimunSecurityTokenValidator(new AuthService(baseAddress)));
            });

            services.AddHangfire(cfg => cfg
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("Hangfire"),
                new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true,
                })
            );
            services.AddHangfireServer();

            services.AddCors();

            services.AddRouting(opt => opt.LowercaseUrls = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "App.Api v1");
                c.InjectStylesheet("/swagger-ui/swagger.min.css");
                c.InjectJavascript("/swagger-ui/swagger.min.js");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
