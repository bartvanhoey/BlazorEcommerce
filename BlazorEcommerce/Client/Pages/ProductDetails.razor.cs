using Client.Services.Products;

namespace BlazorEcommerce.Client.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Inject] protected IProductService? ProductService { get; set; }
        [Inject] protected ICartService? CartService { get; set; }

        protected Product? Product = null;
        protected string message = string.Empty;

        protected int CurrentTypeId = 1;

        [Parameter] public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            message = "Loading products";
            var result = await ProductService!.GetProductAsync(Id)!;
            if (result.Success)
            {
                Product = result.Data;
                if (Product != null && Product.Variants.Count > 0)
                {
                    CurrentTypeId = Product.Variants[0].ProductTypeId;
                }
            }
            else
                message = result.Message;
        }

        protected ProductVariant GetProductVariant(){
            var variant = Product?.Variants.FirstOrDefault(v => v.ProductTypeId == CurrentTypeId);
            return variant!;
        }

        public async Task AddToCartAsync()
        {
            var productVariant = GetProductVariant();
            var cartItem = new CartItem{
                ProductId = productVariant.ProductId,
                ProductTypeId = productVariant.ProductTypeId
            };

            await CartService?.AddToCartAsync(cartItem)!;
        }









    }
}