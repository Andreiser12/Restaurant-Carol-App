namespace RestaurantCarol.Infrastructure.Services;

public class TokenService : Security.TokenService
{
    public TokenService(Microsoft.Extensions.Configuration.IConfiguration configuration) : base(configuration)
    {
    }
}
