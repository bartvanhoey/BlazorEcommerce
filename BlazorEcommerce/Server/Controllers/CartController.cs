using Microsoft.AspNetCore.Mvc;
using Server.Services.Cart;
using Shared;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService) => _cartService = cartService;

        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartProducts(List<CartItem> cartItems) 
            => Ok(await _cartService.GetCartProductsAsync(cartItems));

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> StoreCartItems(List<CartItem> cartItems) 
            => Ok(await _cartService.StoreCartAsync(cartItems));

        [HttpPut("update-quantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(CartItem cartItem) 
            => Ok(await _cartService.UpdateQuantityAsync(cartItem));

        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem) 
            => Ok(await _cartService.AddToCartAsync(cartItem));

        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveItemFromCart(int productId, int productTypeId) 
            => Ok(await _cartService.RemoveItemFromCartAsync(productId, productTypeId));


        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
            => await _cartService.GetCartItemsCountAsync();

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetDbCartProducts()
            => Ok(await _cartService.GetDbCartProductsAsync());

    }
}