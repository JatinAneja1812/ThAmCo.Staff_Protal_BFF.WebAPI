using DTOs.UserProfiles;
using Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Interfaces.Customers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Service.Classes.Users
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;

        public UserService(IHttpClientFactory clientFactory, ILogger<UserService> logger, IConfiguration Configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = Configuration;
        }
        public async Task<List<UserProfilesDTO>> GetAllCustomers(string? accessToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                // Get the base address from configuration
                var apiBaseAddress = _configuration["Services:UserProfiles:BaseAddress"];
                client.BaseAddress = new Uri(apiBaseAddress);

                // Set the authorization header with the access token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Construct the full API endpoint for fetching all customers
                var endpoint = _configuration["Services:UserProfiles:GetAllCustomersEndpoint"];
                var response = await client.GetAsync(endpoint);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read and parse the response content into a List<UserProfilesDTO>
                var result = await response.Content.ReadFromJsonAsync<List<UserProfilesDTO>>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    new EventId((int)LogEventIdEnum.UnknownError),
                    $"Unexpected exception was caught in UserService at GetAllCustomers().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                // You may want to handle or log the exception further
                throw;
            }
        }
    }
}
