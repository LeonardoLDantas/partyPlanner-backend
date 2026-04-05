using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PartyPlanner.Infrastructure.Data;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PartyPlannerDbContext>
{
    public PartyPlannerDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        if (Path.GetFileName(basePath).Equals("PartyPlanner.WebApi", StringComparison.OrdinalIgnoreCase))
        {
            return CreateFromBasePath(basePath);
        }

        var webApiPath = Path.Combine(basePath, "src", "PartyPlanner.WebApi");
        if (Directory.Exists(webApiPath))
        {
            return CreateFromBasePath(webApiPath);
        }

        var infrastructurePath = Path.Combine(basePath, "src", "PartyPlanner.Infrastructure");
        if (Directory.Exists(infrastructurePath))
        {
            return CreateFromBasePath(Path.Combine(basePath, "src", "PartyPlanner.WebApi"));
        }

        throw new InvalidOperationException("Unable to locate the PartyPlanner.WebApi configuration files for EF Core tooling.");
    }

    private static PartyPlannerDbContext CreateFromBasePath(string basePath)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        var optionsBuilder = new DbContextOptionsBuilder<PartyPlannerDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            npgsql => npgsql.MigrationsAssembly(typeof(PartyPlannerDbContext).Assembly.FullName)
        );

        return new PartyPlannerDbContext(optionsBuilder.Options);
    }
}
