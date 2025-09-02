using EasyDine.Data;
using EasyDine.Repositories;
using EasyDine.Repositories.Bookings;
using EasyDine.Services;
using Microsoft.EntityFrameworkCore;

namespace EasyDine.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEasyDineCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>
            (o => o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<BookingService>();
        services.AddScoped<AvailabilityService>();
        services.AddScoped<AuthService>();
        services.Configure<BookingRulesOptions>(configuration.GetSection("BookingRules"));
        return services;
    }
}