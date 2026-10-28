using Microsoft.Extensions.DependencyInjection;
using RestaurantCarol.Domain.Interfaces;
using RestaurantCarol.Infrastructure.Security;

namespace RestaurantCarol.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}
