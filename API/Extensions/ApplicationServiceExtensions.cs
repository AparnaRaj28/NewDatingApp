using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interface;
using API.Service;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        //creating an extension method
        public static IServiceCollection AddApplicationServices
                (this IServiceCollection services,IConfiguration config)
        {
             services.AddDbContext<DataContext>(options=>{
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
                });

             services.AddCors();

             services.AddScoped<ITokenService, TokenService>();
             services.AddScoped<IUserRepository,UserRepository>();
             services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

             return services;
        }
    }
}