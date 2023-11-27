using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services.Addresses
{
    public interface IAddressService
    {
        Task<Address> GetAddressAsync();
        Task<Address> AddOrdUpdateAddressAsync(Address address);
    }

    public class AddressService : IAddressService
    {
        private readonly HttpClient _http;

        public AddressService(HttpClient client)
        {
            _http = client;
            
        }
        public async Task<Address> AddOrdUpdateAddressAsync(Address address)
        {
            var response = await _http.PostAsJsonAsync("api/address", address);
            return (await response.Content.ReadFromJsonAsync<ServiceResponse<Address>>()).Data;
        }

        public async Task<Address> GetAddressAsync()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<Address>>("api/address");
            return response.Data;

        }
    }
}