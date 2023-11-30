using System.Text;
using Dat_api.Data;
using Dat_api.Helpers;
using Dat_api.Interfaces;
using Dat_api.Services;
using Dat_api.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dat_api.Extentions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {


            //services.AddDbContext<DataContext>(opt =>
            //{
            //    opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            //});


            services.AddCors();


            // AddScoped creates a new instance of the service for each HTTP request (Another option is AddSingleton)
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();



            return services;
        }

    }
}
