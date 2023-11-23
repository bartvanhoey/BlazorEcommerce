using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services.Orders
{
    public interface IOrderService
    {
        Task<string> PlaceOrderAsync();
        Task<List<OrderOverviewResponse>> GetOrdersAsync();
        Task<OrderDetailsResponse?> GetOrderDetailsAsync(int orderId);
    }
}