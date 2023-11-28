using DTOs.UserProfiles;

namespace Service.Interfaces.Customers
{
    public interface IUserService
    {
        public Task<List<UserProfilesDTO>> GetAllCustomers(string? accessToken);
    }
}
