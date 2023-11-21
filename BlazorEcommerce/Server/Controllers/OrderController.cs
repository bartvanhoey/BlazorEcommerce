using System.Runtime.CompilerServices;
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

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder()
            => Ok(await _orderService.PlaceOrderAsync());


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewResponse>>>> GetOrders() 
            => Ok(await _orderService.GetOrdersAsync());



    }
}