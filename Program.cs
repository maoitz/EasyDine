using EasyDine.Data;
using EasyDine.Middleware;
using EasyDine.Repositories;
using EasyDine.Repositories.Bookings;
using EasyDine.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using EasyDine.Filters;

namespace EasyDine;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        // Register the generic repository
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<MenuService>();
        builder.Services.Configure<BookingRulesOptions>(
            builder.Configuration.GetSection("BookingRules"));
        builder.Services.AddScoped<AvailabilityService>();
        builder.Services.AddScoped<BookingService>();
        builder.Services.AddScoped<AuthService>();
        
        var jwt = builder.Configuration.GetSection("Jwt");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,    
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt["issuer"],
                    ValidAudience = jwt["audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["key"]!))
                };
            });
        
        builder.Services.AddAuthorization();
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new RequireAdminForWritesConvention());
        });
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EasyDine API", Version = "v1" });

            // ✅ Use HTTP Bearer so Swagger sends "Authorization: Bearer <token>"
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.\n\nExample: Bearer 12345abcdef",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            // keep your AuthorizeCheckOperationFilter if you added it
            c.OperationFilter<AuthorizeCheckOperationFilter>();
        });
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        app.Run();
    }
}