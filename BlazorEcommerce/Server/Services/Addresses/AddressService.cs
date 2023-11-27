
using Server.Data;
using Server.Services.Auth;
using Shared;

namespace Server.Services.Addresses
{
    public class AddressService : IAddressService
    {
        private readonly IAuthService _authService;
        private readonly DatabaseContext _db;

        public AddressService(DatabaseContext context, IAuthService authService)
        {
            _db = context;
            _authService = authService;

        }

        public async Task<ServiceResponse<Address>> AddOrUpdateAddressAsync(Address address)
        {
            var response = new ServiceResponse<Address>();
            var dbAddress = (await GetAddressAsync()).Data;

            if (dbAddress == null)
            {
                address.UserId = _authService.GetUserId();
                _db.Addresses.Add(address);
                response.Data = address;
            }
            else
            {
                dbAddress.FirstName = address.FirstName;
                dbAddress.LastName = address.LastName;
                dbAddress.Street = address.Street;
                dbAddress.City = address.City;
                dbAddress.ZipCode = address.ZipCode;
                dbAddress.State = address.State;
                dbAddress.Country = address.Country;
            }

            await _db.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<Address>> GetAddressAsync()
        {
            int userId = _authService.GetUserId();
            var address = await _db.Addresses.FirstOrDefaultAsync(x => x.UserId == userId);

            return new ServiceResponse<Address> { Data = address };

        }
    }
}