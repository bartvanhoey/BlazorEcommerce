using Microsoft.AspNetCore.Mvc;
using Server.Services.Orders;
using Shared;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService) => _orderService = orderService;

        public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder()
            => Ok(await _orderService.PlaceOrderAsync());


    }
}