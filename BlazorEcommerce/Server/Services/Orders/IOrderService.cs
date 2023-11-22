using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Server.Services.Orders
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrderAsync();
        Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrdersAsync();
        Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetailsAsync(int orderId);
    }


}


