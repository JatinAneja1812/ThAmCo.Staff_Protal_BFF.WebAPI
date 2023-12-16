using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Service.Classes.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UserProfilesServiceTests
{
    public class RemoveCustomersTest
    {
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public RemoveCustomersTest()
        {
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _configurationMock = new Mock<IConfiguration>();
        }

        [Fact]
        public async Task RemoveCustomers_SuccessfulRequest()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var userId = "user123";
            var expectedBaseAddress = "https://thamco_users_azureexample.net/api/";
            var expectedEndpoint = "users/RemoveCustomers";

            _configurationMock.SetupGet(c => c["Services:UserProfiles:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:UserProfiles:RemoveCustomersEndpoint"]).Returns(expectedEndpoint);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(expectedBaseAddress)
            };

            _clientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var userService = new UserService(_clientFactoryMock.Object, _loggerMock.Object, _configurationMock.Object);

            // Act
            var result = await userService.RemoveCustomers(accessToken, userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveCustomers_UnsuccessfulRequest()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var userId = "user123";
            var expectedException = new HttpRequestException("Simulated error");
            var expectedBaseAddress = "https://thamco_users_azureexample.net/api/";
            var expectedEndpoint = "users/RemoveCustomers";

            _configurationMock.SetupGet(c => c["Services:UserProfiles:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:UserProfiles:RemoveCustomersEndpoint"]).Returns(expectedEndpoint);

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
            var result = await Assert.ThrowsAsync<HttpRequestException>(() => userService.RemoveCustomers(accessToken, userId));
            Assert.Equal(expectedException, result);
        }

        [Fact]
        public async Task RemoveCustomers_UnsuccessfulStatusCode()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var userId = "user123";
            var expectedBaseAddress = "https://thamco_users_azureexample.net/api/";
            var expectedEndpoint = "users/RemoveCustomers";

            _configurationMock.SetupGet(c => c["Services:UserProfiles:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:UserProfiles:RemoveCustomersEndpoint"]).Returns(expectedEndpoint);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(expectedBaseAddress)
            };

            _clientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var userService = new UserService(_clientFactoryMock.Object, _loggerMock.Object, _configurationMock.Object);

            // Act
            var result = await userService.RemoveCustomers(accessToken, userId);

            // Assert
            Assert.False(result);
        }
    }
}
