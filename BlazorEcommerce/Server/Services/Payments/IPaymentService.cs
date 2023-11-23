using Client.Services.Orders;
using Stripe.Checkout;

namespace Server.Services.Payments
{
    public interface IPaymentService
    {
        Task<Session?> CreateCheckoutSessionAsync();       
    }
}