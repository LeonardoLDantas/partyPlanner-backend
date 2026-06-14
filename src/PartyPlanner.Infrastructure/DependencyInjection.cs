using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartyPlanner.Application.Interfaces;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;
using PartyPlanner.Infrastructure.Data;
using PartyPlanner.Infrastructure.Email;
using PartyPlanner.Infrastructure.Repository;
using PartyPlanner.Infrastructure.Security;
using PartyPlanner.Infrastructure.Services;
using Resend;

namespace PartyPlanner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PartyPlannerDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsAssembly(typeof(PartyPlannerDbContext).Assembly.FullName)
            ));

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IPartyRepository, PartyRepository>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IGoogleTokenVerifier, GoogleTokenVerifier>();
        services.AddScoped<DbSeeder>();

        services.AddSingleton<IDateTimeProvider, BrazilDateTimeProvider>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Resend e-mail
        services.AddResend(configuration["Resend:ApiToken"]!);
        services.AddScoped<IEmailSender, ResendEmailSender>();

        return services;
    }
}
