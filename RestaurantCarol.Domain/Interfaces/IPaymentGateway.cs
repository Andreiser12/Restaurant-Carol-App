namespace RestaurantCarol.Domain.Interfaces;

public interface IPaymentGateway
{
    Task<string> CreateCheckoutSessionAsync(decimal amount, string currency, string successUrl, string cancelUrl);
}
