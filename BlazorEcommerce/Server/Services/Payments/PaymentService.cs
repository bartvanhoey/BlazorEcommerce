
using Server.Services.Auth;
using Server.Services.Cart;
using Server.Services.Orders;
using Shared;
using Stripe;
using Stripe.Checkout;

namespace Server.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;
        const string secret = "whsec_cfaa03e8c9809dde410c396ff0d8fdec130f7bf3051efe93ba93552a3f263fa2";

        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService)
        {
            StripeConfiguration.ApiKey = "sk_test_51KH7mHIni9JOE5dI0o8yJRfv8wznCU8p7tmCCXwc5mRrkSVD26y6p5S344twZTkT7zaS23FeBSuJS8KAaxDtzrzm00ei9dQfkh";
            _orderService = orderService;
            _authService = authService;
            _cartService = cartService;
        }

        public async Task<Session?> CreateCheckoutSessionAsync()
        {
            var products = (await _cartService.GetDbCartProductsAsync()).Data;
            if (products == null || products.Count == 0) return null;

            var lineItems = new List<SessionLineItemOptions>();
            products.ForEach(product => lineItems.Add(new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl },
                    }
                },
                Quantity = product.Quantity
            }));

            var customerEmail = _authService.GetUserEmail();

            var options = new SessionCreateOptions
            {
                
                CustomerEmail = customerEmail,
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7133/order-success",
                CancelUrl = "https://localhost:7133/cart"
            };

            var sessionService = new SessionService();
            Session session = sessionService.Create(options);

            return session;
        }

        public async Task<ServiceResponse<bool>> FulFillOrderAsync(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    request.Headers["Stripe-Signature"],
                    secret
                );

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmailAsync(session.CustomerDetails.Email);
                    await _orderService.PlaceOrderAsync(user.Id);
                }

                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException e)
            {
                return new ServiceResponse<bool> { Data = false, Success = false, Message = e.Message };
            }
        }
    }
}
