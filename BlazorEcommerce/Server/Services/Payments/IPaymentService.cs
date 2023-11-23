using Client.Services.Orders;
using Shared;
using Stripe.Checkout;

namespace Server.Services.Payments
{
    public interface IPaymentService
    {
        Task<Session?> CreateCheckoutSessionAsync();   
        Task<ServiceResponse<bool>> FulFillOrderAsync(HttpRequest request);   

    }
}