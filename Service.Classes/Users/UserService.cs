using DTOs.UserProfiles;
using Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Interfaces.Customers;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

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

                var apiBaseAddress = _configuration["Services:UserProfiles:BaseAddress"];
                client.BaseAddress = new Uri(apiBaseAddress);

                // Setted the authorization header with the access token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Constructed the full API endpoint for fetching all customers
                var endpoint = _configuration["Services:UserProfiles:GetAllCustomersEndpoint"];

                // Send the GET request
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

        public async Task<bool> RemoveCustomers(string? accessToken, string userId)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var apiBaseAddress = _configuration["Services:UserProfiles:BaseAddress"];
                client.BaseAddress = new Uri(apiBaseAddress);

                // Set the authorization header with the access token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Construct the full API endpoint for removing a user
                var endpoint = $"{_configuration["Services:UserProfiles:RemoveCustomersEndpoint"]}";

                // Create a DELETE request with a request body
                client.DefaultRequestHeaders.Add("UserId", userId);
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
                    $"Unexpected exception was caught in UserService at RemoveCustomers().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                // You may want to handle or log the exception further
                throw;
            }
        }
    }
}
