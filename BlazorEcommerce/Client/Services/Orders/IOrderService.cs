using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services.Orders
{
    public interface IOrderService
    {
        Task PlaceOrderAsync();
    }
}