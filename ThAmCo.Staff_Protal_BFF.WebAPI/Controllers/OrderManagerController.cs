using DTOs.Orders;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Classes;
using Service.Interfaces.Orders;

namespace ThAmCo.Staff_Protal_BFF.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderManagerController : Controller
    {
        private readonly IOrdersService _orders;
        private readonly ILogger<OrderManagerController> _logger;
        private readonly ITokenService _tokenService;

        public OrderManagerController(IOrdersService Orders, ITokenService tokenService, ILogger<OrderManagerController> Logger)
        {
            _orders = Orders;
            _tokenService = tokenService;
            _logger = Logger;
        }

        [Authorize]
        [HttpPost]
        [Route("AddNewOrderByStaff")]
        public async Task<ActionResult<bool>> AddNewOrderByStaff(AddNewOrderDTO addNewOrderDTO)
        {
            try
            {

                var orderAPItoken = await _tokenService.GetOrdersAPIAccessTokenAsync();  //Authorization.Split(" ")[1];
                var usersAPItoken = await _tokenService.GetUserProfilesAPIAccessTokenAsync();  //Authorization.Split(" ")[1];

                var res = orderAPItoken.access_token.Equals(usersAPItoken.access_token);

                var result = await _orders.AddNewOrder(orderAPItoken.access_token, usersAPItoken.access_token, addNewOrderDTO);

                if (result == false)
                {
                    return StatusCode(500, "Failed to add newly created order to the database.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in OrderManagerController at AddNewOrder().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("bbg")]
        public async Task<ActionResult<bool>> Ajs()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in OrderManagerController at AddNewOrder().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }
    }
}
