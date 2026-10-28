using Microsoft.Extensions.Configuration;
using RestaurantCarol.Domain.Interfaces;

namespace RestaurantCarol.Gateway.Payment;

public class StripePaymentGateway : IPaymentGateway
{
    private readonly IConfiguration _configuration;

    public StripePaymentGateway(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string> CreateCheckoutSessionAsync(decimal amount, string currency, string successUrl, string cancelUrl)
    {
        // Mocked checkout session URL for initial development and testing
        var mockSessionId = Guid.NewGuid().ToString("N");
        var checkoutUrl = $"{successUrl}?session_id={mockSessionId}&amount={amount}&currency={currency}";
        return Task.FromResult(checkoutUrl);
    }
}
