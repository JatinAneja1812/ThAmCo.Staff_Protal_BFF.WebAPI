using DTOs.Customers;
using DTOs.UserProfiles;

namespace Service.Interfaces.Customers
{
    public interface IUserService
    {
        public Task<List<UserProfilesDTO>> GetAllCustomers(string? accessToken);
        public Task<UserProfilesDTO> GetCurrentlyLoggedInStaff(string? accessToken, string email);
        public Task<bool> RemoveCustomers(string? accessToken, string userId);
        public Task<bool> UpdateCustomersFunds(string? accessToken, CustomerFundsDTO customerFunds);
        public Task<bool> DirectAlterCustomersFunds(string? accessToken, CustomerFundsDTO customerFunds);
    }
}
