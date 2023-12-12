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
                    $"Unexpected exception was caught in OrderManagerController at AddNewOrderByStaff().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllOrders")]
        public async Task<ActionResult<bool>> GetAllOrders()
        {
            try
            {
                var orderAPItoken = await _tokenService.GetOrdersAPIAccessTokenAsync();  //Authorization.Split(" ")[1];

                var result = await _orders.GetAllOrders(orderAPItoken.access_token);

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in OrderManagerController at GetAllOrders().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllHistoricOrders")]
        public async Task<ActionResult<bool>> GetAllHistoricOrders()
        {
            try
            {
                var orderAPItoken = await _tokenService.GetOrdersAPIAccessTokenAsync();  //Authorization.Split(" ")[1];

                var result = await _orders.GetAllOrders(orderAPItoken.access_token);

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in OrderManagerController at GetAllHistoricOrders().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetOrdersCount")]
        public async Task<ActionResult<bool>> GetOrdersCount()
        {
            try
            {
                var orderAPItoken = await _tokenService.GetOrdersAPIAccessTokenAsync();  //Authorization.Split(" ")[1];

                var result = await _orders.GetAllOrders(orderAPItoken.access_token);

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in OrderManagerController at GetOrdersCount().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("CancelOrder")]
        public async Task<ActionResult<bool>> RemoveExistingorder([FromHeader] string orderId)
        {
            try
            {
                var orderAPItoken = await _tokenService.GetOrdersAPIAccessTokenAsync();  //Authorization.Split(" ")[1];

                var result = await _orders.RemoveOrder(orderAPItoken.access_token, orderId);

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in OrderManagerController at RemoveExistingorder().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }

        [Authorize]
        [HttpPatch]
        [Route("UpdateOrderStatus")]
        public async Task<ActionResult<bool>> UpdateOrderStatus([FromHeader] OrderStatusDTO order)
        {
            try
            {
                var orderAPItoken = await _tokenService.GetOrdersAPIAccessTokenAsync();  //Authorization.Split(" ")[1];

                var result = await _orders.UpdateOrderStatus(orderAPItoken.access_token, order);

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in OrderManagerController at UpdateOrderStatus().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }

        [Authorize]
        [HttpPatch]
        [Route("UpdateOrderDeliveryDate")]
        public async Task<ActionResult<bool>> UpdateOrderDeliveryDate([FromHeader] ScheduledOrderDTO order)
        {
            try
            {
                var orderAPItoken = await _tokenService.GetOrdersAPIAccessTokenAsync();  //Authorization.Split(" ")[1];

                var result = await _orders.UpdateOrderDeliveryDate(orderAPItoken.access_token, order);

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in OrderManagerController at UpdateOrderDeliveryDate().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server.");
            }
        }

    }
}
