namespace Client.Services.Addresses
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _http;

        public AddressService(HttpClient client) 
            => _http = client;
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