using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  BlazorEcommerce.Client.Shared
{
    public class FeaturedProductsBase :  ComponentBase
    {
        [Parameter]
        public List<Product>? Products { get; set; }
    }
}