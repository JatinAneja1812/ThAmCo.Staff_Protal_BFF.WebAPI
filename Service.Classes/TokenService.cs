﻿using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using static Service.Classes.TokenService;

namespace Service.Classes
{
    public interface ITokenService
    {
        public Task<TokenDto> GetUserProfilesAPIAccessTokenAsync();
        public Task<TokenDto> GetOrdersAPIAccessTokenAsync();
    }

    public class TokenService : ITokenService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public TokenService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public record TokenDto(string access_token, string token_type, int expires_in);

        //User Profiles web api access token
        public async Task<TokenDto> GetUserProfilesAPIAccessTokenAsync()
        {
            var tokenClient = _clientFactory.CreateClient();

            var authBaseAddress = _configuration["Auth:Authority"];
            tokenClient.BaseAddress = new Uri(authBaseAddress);

            var tokenParams = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _configuration["Auth:ClientId"] },
                { "client_secret", _configuration["Auth:ClientSecret"] },
                { "audience", _configuration["Auth:Audience_UserProfiles"] }, // Corrected key
            };

            var tokenForm = new FormUrlEncodedContent(tokenParams);

            var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenForm);

            tokenResponse.EnsureSuccessStatusCode();

            var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

            return tokenInfo;
        }

        //Orders web api access token
        public async Task<TokenDto> GetOrdersAPIAccessTokenAsync()
        {
            var tokenClient = _clientFactory.CreateClient();

            var authBaseAddress = _configuration["Auth:Authority"];
            tokenClient.BaseAddress = new Uri(authBaseAddress);

            var tokenParams = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _configuration["Auth:ClientId"] },
                { "client_secret", _configuration["Auth:ClientSecret"] },
                { "audience", _configuration["Auth:Audience_Orders"] }, // Corrected key
            };

            var tokenForm = new FormUrlEncodedContent(tokenParams);

            var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenForm);

            tokenResponse.EnsureSuccessStatusCode();

            var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

            return tokenInfo;
        }
    }
}