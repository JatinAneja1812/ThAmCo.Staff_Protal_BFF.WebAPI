using DTOs.Customers;
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

                var customers = await _usersProfiles.GetAllCustomers(token);

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

        [Authorize]
        [HttpDelete]
        [Route("RemoveCustomer")]
        public async Task<ActionResult<bool>> RemoveCustomers([FromHeader] string Authorization, [FromHeader] string UserId)
        {
            try
            {
                var token = Authorization.Split(" ")[1];

                bool result = await _usersProfiles.RemoveCustomers(token, UserId);

                if (result == false)
                {
                    return StatusCode(500, "Failed to remove customer from the database.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in UsersController at GetAllCustomers().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }

        [Authorize]
        [HttpPatch]
        [Route("UpdateCustomerFunds")]
        public async Task<ActionResult<bool>> UpdateCustomerFunds([FromHeader] string Authorization, CustomerFundsDTO customerFunds)
        {
            try
            {
                var token = Authorization.Split(" ")[1];

                var result = await _usersProfiles.UpdateCustomersFunds(token, customerFunds);

                if (result == false)
                {
                    return StatusCode(500, "Failed to update customer funds to the database.");
                }

                return Ok(result);
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
