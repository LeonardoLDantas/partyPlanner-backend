using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartyPlanner.Application.Interface;
using PartyPlanner.Infrastructure.Data;
using PartyPlanner.Infrastructure.Repository;
using PartyPlanner.Infrastructure.Security;

namespace PartyPlanner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PartyPlannerDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlServer => sqlServer.MigrationsAssembly(typeof(PartyPlannerDbContext).Assembly.FullName)
            ));

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IPartyRepository, PartyRepository>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IGoogleTokenVerifier, GoogleTokenVerifier>();
        services.AddScoped<DbSeeder>();

        return services;
    }
}
