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
    public class GetCurrentlyLoggedInStaffTest
    {
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public GetCurrentlyLoggedInStaffTest()
        {
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _configurationMock = new Mock<IConfiguration>();
        }

        [Fact]
        public async Task GetCurrentlyLoggedInStaff_SuccessfulRequest()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var email = "staff@example.com";
            var expectedUser = new UserProfilesDTO
            {
                UserId = Guid.NewGuid().ToString(),
                Username = "staff_user",
                Email = email,
                FirstName = "Staff",
                LastName = "User",
                PhoneNumber = "123-456-7890",
                AvailableFunds = 100.0,
                UserType = UserTypeEnum.Staff,
                LocationNumber = "LOC001",
                Street = "Staff Street",
                City = "Staff City",
                State = "Staff State",
                PostalCode = "12345",
                UserAddedOnDate = DateTime.Now
            };
            var expectedJson = JsonContent.Create(expectedUser);
            var expectedBaseAddress = "https://thamco_users_azureexample.net/api/";
            var expectedEndpoint = "users/GetStaffDetails";

            _configurationMock.SetupGet(c => c["Services:UserProfiles:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:UserProfiles:GetStaffDetailsEndpoint"]).Returns(expectedEndpoint);

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
            var result = await userService.GetCurrentlyLoggedInStaff(accessToken, email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Username, result.Username);
            Assert.Equal(expectedUser.UserType, result.UserType);
            Assert.Equal(expectedUser.UserId, result.UserId);
        }

        [Fact]
        public async Task GetCurrentlyLoggedInStaff_ExceptionHandled()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var email = "staff@example.com";
            var expectedException = new HttpRequestException("Simulated error");
            var expectedBaseAddress = "https://thamco_users_azureexample.net/api/";
            var expectedEndpoint = "users/GetStaffDetails";

            _configurationMock.SetupGet(c => c["Services:UserProfiles:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:UserProfiles:GetStaffDetailsEndpoint"]).Returns(expectedEndpoint);

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
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => userService.GetCurrentlyLoggedInStaff(accessToken, email));
            Assert.Equal(expectedException, exception);
        }
    }
}
