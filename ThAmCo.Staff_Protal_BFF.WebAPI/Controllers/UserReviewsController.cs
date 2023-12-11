using DTOs.UserReviews;
using Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Interfaces.UserReviews;

namespace ThAmCo.Staff_Protal_BFF.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReviewsController : Controller
    {
        private readonly ICustomerReviews _reviews;
        private readonly ILogger<UserReviewsController> _logger;

        public UserReviewsController(ICustomerReviews Reviews, ILogger<UserReviewsController> logger)
        {
            _reviews = Reviews;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllReviews")]
        public async Task<ActionResult<List<UsersReviewDto>>> GetAllReviews()
        {
            try
            {
                var result = await _reviews.GetUsersReviewAsync();

                if (result == null)
                {
                    return StatusCode(500, "Failed to retrieve customer reviews from the database.");
                }

                return Ok(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                 new EventId((int)LogEventIdEnum.UnknownError),
                 $"Unexpected exception was caught in CustomersController.\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server..");
            }
        }
    }
}
