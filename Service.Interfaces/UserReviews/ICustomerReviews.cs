using DTOs;

namespace Service.Interfaces.UserReviews
{
    public interface ICustomerReviews
    {
        public Task<List<UsersReviewDto>> GetUsersReviewAsync(int numberOfUsers = 9);
    }
}
