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
using PartyPlanner.Application.Common;
using PartyPlanner.Infrastructure.Services;

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

        // App options
        var appOptions = configuration.GetSection("App").Get<AppOptions>() ?? new AppOptions();
        services.AddSingleton(appOptions);

        // Gmail SMTP
        var fromEmail = configuration["Email:FromAddress"]!;
        var appPassword = configuration["Email:AppPassword"]!;
        services.AddScoped<IEmailSender>(_ => new GmailEmailSender(fromEmail, appPassword));

        return services;
    }
}
