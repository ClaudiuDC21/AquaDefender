using AquaDefender_Backend.Data;
using AquaDefender_Backend.Repositories;
using AquaDefender_Backend.Repository;
using AquaDefender_Backend.Repository.Interfaces;
using AquaDefender_Backend.Service;
using AquaDefender_Backend.Service.Interfaces;
using AquaDefender_Backend.Services;
using AquaDefender_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.ApplicationInsights.AspNetCore;

namespace AquaDefender_Backend.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddApplicationInsightsTelemetry(options =>
            {
                options.ConnectionString = config["ApplicationInsights:ConnectionString"];
            });

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IWaterInfoService, WaterInfoService>();
            services.AddScoped<IWaterValuesService, WaterValuesService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IWaterInfoRepository, WaterInfoRepository>();
            services.AddScoped<IWaterValuesRepository, WaterValuesRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();

            services.AddDbContext<AquaDefenderDataContext>(options =>
                       options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}