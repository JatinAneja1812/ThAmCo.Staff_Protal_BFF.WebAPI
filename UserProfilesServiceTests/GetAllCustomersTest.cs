using DTOs.UserProfiles;
using Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Service.Classes.Users;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace UserProfilesServiceTests
{
    public class GetAllCustomersTest
    {
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public GetAllCustomersTest()
        {
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _configurationMock = new Mock<IConfiguration>();
        }

        private List<UserProfilesDTO> GetFakeUserProfiles(int count)
        {
            var fakeUserProfiles = new List<UserProfilesDTO>();
            for (int i = 0; i < count; i++)
            {
                var userProfile = new UserProfilesDTO
                {
                    UserId = Guid.NewGuid().ToString(),
                    Username = $"user{i}",
                    Email = $"user{i}@example.com",
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}",
                    PhoneNumber = $"123-456-{i.ToString("D4")}",
                    AvailableFunds = 100.0 + i * 10,
                    UserType = UserTypeEnum.Customer,
                    LocationNumber = $"LOC{i}",
                    Street = $"Street{i}",
                    City = $"City{i}",
                    State = $"State{i}",
                    PostalCode = $"12345{i}",
                    UserAddedOnDate = DateTime.Now.AddDays(-i)
                };

                fakeUserProfiles.Add(userProfile);
            }

            return fakeUserProfiles;
        }

        [Fact]
        public async Task GetAllCustomers_SuccessfulRequest()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var expectedUserProfiles = GetFakeUserProfiles(4);
            var expectedJson = JsonContent.Create(expectedUserProfiles);
            var expectedBaseAddress = "https://thamco_users_azureexample.net/api/";
            var expectedEndpoint = "users/GetAllUsers";

            _configurationMock.SetupGet(c => c["Services:UserProfiles:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:UserProfiles:GetAllCustomersEndpoint"]).Returns(expectedEndpoint);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = expectedJson
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(expectedBaseAddress)
            };

            _clientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var userService = new UserService(_clientFactoryMock.Object, _loggerMock.Object, _configurationMock.Object);

            // Act
            var result = await userService.GetAllCustomers(accessToken);

            // Assert
            Assert.Equal(expectedUserProfiles.Count, result.Count);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllCustomers_ExceptionHandled()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var expectedException = new HttpRequestException("Simulated error");
            var expectedBaseAddress = "https://thamco_users_azureexample.net/api/";
            var expectedEndpoint = "users/GetAllUsers";

            _configurationMock.SetupGet(c => c["Services:UserProfiles:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:UserProfiles:GetAllCustomersEndpoint"]).Returns(expectedEndpoint);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(expectedException);

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://example.com/api/")
            };

            _clientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var userService = new UserService(_clientFactoryMock.Object, _loggerMock.Object, _configurationMock.Object);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => userService.GetAllCustomers(accessToken));
            Assert.Equal(expectedException, exception);
        }
    }
}
