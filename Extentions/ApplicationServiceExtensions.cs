using System.Text;
using Dat_api.Data;
using Dat_api.Interfaces;
using Dat_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dat_api.Extentions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // AddScoped creates a new instance of the service for each HTTP request (Another option is AddSingleton)
            services.AddScoped<ITokenService, TokenService>();

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();


            return services;
        }

    }
}
