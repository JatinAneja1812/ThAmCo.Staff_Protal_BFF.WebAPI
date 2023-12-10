using DTOs.Customers;
using DTOs.Orders;
using Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Interfaces.Company;
using Service.Interfaces.Customers;
using Service.Interfaces.Orders;
using System.Net.Http.Headers;
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

                    _userService.UpdateCustomersFunds(usersAccessToken, updatedFunds);

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
                     $"Unexpected exception was caught in OrdersService at AddNewOrder().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");
                throw;
            }
        }
    }
}
