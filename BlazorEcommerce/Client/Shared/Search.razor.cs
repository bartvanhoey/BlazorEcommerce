using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorEcommerce.Client.Shared
{
    public class SearchBase : ComponentBase
    {

        [Inject] public NavigationManager? NavigationManager { get; set; }
        [Inject] public IProductService? ProductService { get; set; }

        protected string SearchText = string.Empty;
        protected List<string> Suggestions = new();
        protected ElementReference SearchInput;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SearchInput.FocusAsync();
            }
        }

        public void SearchProducts()
        {
            NavigationManager!.NavigateTo($"search/{SearchText}/1", false);
        }

        public async Task HandleSearch(KeyboardEventArgs args)
        {

            if (args.Key == null || args.Key.Equals("Enter"))
            {
                SearchProducts();
            }
            else if (SearchText.Length > 1)
            {
                Suggestions = await ProductService!.GetProductSearchSuggestionsAsync(SearchText);
            }

        }



    }
}