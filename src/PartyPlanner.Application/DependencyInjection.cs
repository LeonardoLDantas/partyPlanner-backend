using Microsoft.Extensions.DependencyInjection;
using PartyPlanner.Application.Interface;
using PartyPlanner.Application.Services;

namespace PartyPlanner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPartyService, PartyService>();
        return services;
    }
}
