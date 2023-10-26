using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services.Cart
{
    public interface ICartService
    {
        Task AddToCartAsync(CartItem cartItem);
        Task<List<CartItem>> GetCartItemsAsync();
    }
}