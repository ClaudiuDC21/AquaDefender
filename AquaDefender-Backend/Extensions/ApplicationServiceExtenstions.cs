using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Data;
using AquaDefender_Backend.Repository;
using AquaDefender_Backend.Repository.Interfaces;
using AquaDefender_Backend.Service;
using AquaDefender_Backend.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AquaDefender_Backend.Extensions
{
    public static class ApplicationServiceExtenstions
    {
         public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        //Services
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();

        //Repository
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddDbContext<AquaDefenderDataContext>(options =>
            options.UseSqlServer(@"Server=ASUS-LAPTOPCLAU\SQLEXPRESS;Database=AquaDefenderDatabase;Trusted_Connection=True;Encrypt=False;"));

        return services;
    }
    }
}