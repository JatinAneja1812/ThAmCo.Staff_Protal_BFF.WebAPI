using DTOs.Customers;
using DTOs.UserProfiles;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Classes;
using Service.Interfaces.Customers;

namespace ThAmCo.Staff_Protal_BFF.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService _usersProfiles;
        private readonly ILogger<UsersController> _logger;
        private readonly ITokenService _tokenService; 

        public UsersController(IUserService UsersProfiles,ITokenService tokenService, ILogger<UsersController> logger)
        {
            _usersProfiles = UsersProfiles;
            _tokenService = tokenService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllCustomers")]
        public async Task<ActionResult<List<UserProfilesDTO>>> GetAllCustomers()
        {
            try
            {
                var token = await _tokenService.GetUserProfilesAPIAccessTokenAsync();          //Authorization.Split(" ")[1];

                var customers = await _usersProfiles.GetAllCustomers(token.access_token);

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
        [HttpGet]
        [Route("GetMessages")]
        public async Task<ActionResult<string>> GetMessage()
        {
            try
            {
                //var token = await _tokenService.GetUserProfilesAPIAccessTokenAsync();          //Authorization.Split(" ")[1];

                //var customers = await _usersProfiles.GetAllCustomers(token.access_token);
                var msg = "Hello There.";
                return Ok(msg);
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
        [HttpGet]
        [Route("GetStaffUser")]
        public async Task<ActionResult<UserProfilesDTO>> GetCurrentLoggedInStaff([FromHeader] string Email)
        {
            try
            {
                var token = await _tokenService.GetUserProfilesAPIAccessTokenAsync();          //Authorization.Split(" ")[1];

                UserProfilesDTO staff = await _usersProfiles.GetCurrentlyLoggedInStaff(token.access_token, Email);

                if(staff == null)
                {
                    return StatusCode(500, "Failed to return staff details. Try to login again.");
                }

                return Ok(staff);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in UsersController at GetCurrentLoggedInStaff().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("RemoveCustomer")]
        public async Task<ActionResult<bool>> RemoveCustomers([FromHeader] string UserId)
        {
            try
            {
                var token = await _tokenService.GetUserProfilesAPIAccessTokenAsync();  //Authorization.Split(" ")[1];

                bool result = await _usersProfiles.RemoveCustomers(token.access_token, UserId);

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
                    $"Unexpected exception was caught in UsersController at RemoveCustomers().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }

        [Authorize]
        [HttpPatch]
        [Route("UpdateCustomerFunds")]
        public async Task<ActionResult<bool>> UpdateCustomerFunds(CustomerFundsDTO customerFunds)
        {
            try
            {
                var token = await _tokenService.GetUserProfilesAPIAccessTokenAsync();  //Authorization.Split(" ")[1];

                var result = await _usersProfiles.UpdateCustomersFunds(token.access_token, customerFunds);

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
                    $"Unexpected exception was caught in UsersController at UpdateCustomerFunds().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }


    }
}
