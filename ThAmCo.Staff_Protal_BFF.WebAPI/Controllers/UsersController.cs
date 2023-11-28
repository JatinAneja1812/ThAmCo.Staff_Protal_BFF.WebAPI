using DTOs.UserProfiles;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.Customers;

namespace ThAmCo.Staff_Protal_BFF.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService _usersProfiles;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService UsersProfiles, ILogger<UsersController> logger)
        {
            _usersProfiles = UsersProfiles;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllCustomers")]
        public async Task<ActionResult<List<UserProfilesDTO>>> GetAllCustomers([FromHeader] string Authorization)
        {
            try
            {
                var token = Authorization.Split(" ")[1];
                // Call GetAllCustomers using the obtained access token
                var customers = await _usersProfiles.GetAllCustomers(token);

                // Handle the customers data as needed
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in UsersController at GetAllCustomers().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }
    }
}
