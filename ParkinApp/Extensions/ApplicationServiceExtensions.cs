using Microsoft.EntityFrameworkCore;
using ParkinApp.Data;
using ParkinApp.Interfaces;
using ParkinApp.Services;

namespace ParkinApp.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<ParkingDbContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        }); 
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
    
