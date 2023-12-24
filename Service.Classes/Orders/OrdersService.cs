using DTOs.Customers;
using DTOs.Orders;
using Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Interfaces.Company;
using Service.Interfaces.Orders;
using Service.Interfaces.Users;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Service.Classes.Orders
{
    public class OrdersService : IOrdersService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<OrdersService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;

        public OrdersService(IHttpClientFactory clientFactory, ILogger<OrdersService> Logger, IConfiguration Configuration,
            IUserService UserService, ICompanyService CompanyService)
        {
            _clientFactory = clientFactory;
            _logger = Logger;
            _configuration = Configuration;
            _userService = UserService;
            _companyService = CompanyService;
        }

        public async Task<bool> AddNewOrder(string? orderAccessToken, string? usersAccessToken, AddNewOrderDTO addNewOrderDTO)
        {

            try
            {
                addNewOrderDTO.BillingAddress = _companyService.GetCompanyDetails();

                var client = _clientFactory.CreateClient();

                var apiBaseAddress = _configuration["Services:Order:BaseAddress"];
                client.BaseAddress = new Uri(apiBaseAddress);

                // Setted the authorization header with the access token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", orderAccessToken);

                // Constructed the full API endpoint for removing a user
                var endpoint = $"{_configuration["Services:Order:AddNewOrderEndpoint"]}";

                // Serialized the newOrderDto object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(addNewOrderDTO), Encoding.UTF8, "application/json");

                // Send a Post request with the serialized JSON in the request body
                var response = await client.PostAsync(endpoint, jsonContent);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    var changedFunds = Math.Round(addNewOrderDTO.Customer.AvailableFunds - (double.TryParse(addNewOrderDTO.Total, out double total) ? total : 0.0), 2);
                    CustomerFundsDTO updatedFunds = new CustomerFundsDTO
                    {
                        UserId = addNewOrderDTO.CustomerId,
                        Amount = changedFunds
                    };

                    bool isFundUpdated = await _userService.DirectAlterCustomersFunds(usersAccessToken, updatedFunds);

                    if (!isFundUpdated)
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                     new EventId((int)LogEventIdEnum.UnknownError),
                     $"Unexpected exception was caught in Order Service at AddNewOrder().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");
                throw;
            }
        }

        public async Task<List<OrderDTO>> GetAllOrders(string? accessToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var apiBaseAddress = _configuration["Services:Order:BaseAddress"];
                client.BaseAddress = new Uri(apiBaseAddress);

                // Setted the authorization header with the access token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Constructed the full API endpoint for fetching all customers
                var endpoint = _configuration["Services:Order:GetAllOrdersEndpoint"];

                // Send the GET request
                var response = await client.GetAsync(endpoint);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read and parse the response content into a List<UserProfilesDTO>
                var result = await response.Content.ReadFromJsonAsync<List<OrderDTO>>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in Order Service at GetAllOrders().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");
                throw;
            }
        }

        public async Task<List<OrderDTO>> GetAllHistoricOrders(string? accessToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var apiBaseAddress = _configuration["Services:Order:BaseAddress"];
                client.BaseAddress = new Uri(apiBaseAddress);

                // Setted the authorization header with the access token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Constructed the full API endpoint for fetching all customers
                var endpoint = _configuration["Services:Order:GetAllHistoricOrdersEndpoint"];

                // Send the GET request
                var response = await client.GetAsync(endpoint);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read and parse the response content into a List<UserProfilesDTO>
                var result = await response.Content.ReadFromJsonAsync<List<OrderDTO>>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in Order Service at GetAllOrders().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");
                throw;
            }
        }

        public async Task<int> GetOrdersCount(string? accessToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var apiBaseAddress = _configuration["Services:Order:BaseAddress"];
                client.BaseAddress = new Uri(apiBaseAddress);

                // Setted the authorization header with the access token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Constructed the full API endpoint for fetching all customers
                var endpoint = _configuration["Services:Order:GetAllOrderCountEndpoint"];

                // Send the GET request
                var response = await client.GetAsync(endpoint);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read and parse the response content into a List<UserProfilesDTO>
                var result = await response.Content.ReadFromJsonAsync<int>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in Order Service at GetOrdersCount().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> RemoveOrder(string? accessToken, string orderId)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var apiBaseAddress = _configuration["Services:Order:BaseAddress"];
                client.BaseAddress = new Uri(apiBaseAddress);

                // Set the authorization header with the access token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Construct the full API endpoint for removing a user
                var endpoint = $"{_configuration["Services:Order:RemoveOrdersEndpoint"]}";

                // Create a DELETE request with a request body
                client.DefaultRequestHeaders.Add("OrderId", orderId);
                var response = await client.DeleteAsync(endpoint);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in Orders Service at RemoveOrder().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");
                throw;
            }


        }

        public async Task<bool> UpdateOrderStatus(string? accessToken, OrderStatusDTO order)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var apiBaseAddress = _configuration["Services:Order:BaseAddress"];
                client.BaseAddress = new Uri(apiBaseAddress);

                // Setted the authorization header with the access token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Constructed the full API endpoint for removing a user
                var endpoint = $"{_configuration["Services:Order:UpdateOrderStatusEndpoint"]}";

                // Serialized the customerFunds object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

                // Send a PATCH request with the serialized JSON in the request body
                var response = await client.PatchAsync(endpoint, jsonContent);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                     new EventId((int)LogEventIdEnum.UnknownError),
                     $"Unexpected exception was caught in Orders Service at UpdateOrderStatus().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> UpdateOrderDeliveryDate(string? accessToken, ScheduledOrderDTO order)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var apiBaseAddress = _configuration["Services:Order:BaseAddress"];
                client.BaseAddress = new Uri(apiBaseAddress);

                // Setted the authorization header with the access token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Constructed the full API endpoint for removing a user
                var endpoint = $"{_configuration["Services:Order:UpdateOrderDeliveryDateEndpoint"]}";

                // Serialized the customerFunds object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

                // Send a PATCH request with the serialized JSON in the request body
                var response = await client.PatchAsync(endpoint, jsonContent);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                     new EventId((int)LogEventIdEnum.UnknownError),
                     $"Unexpected exception was caught in Orders Service at UpdateOrderDeliveryDate().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");
                throw;
            }
        }
    }
}
