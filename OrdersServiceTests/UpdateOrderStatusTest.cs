﻿using DTOs.Orders;
using Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Service.Classes.Orders;
using Service.Interfaces.Company;
using Service.Interfaces.Users;
using System.Net;
using Xunit;

namespace OrdersServiceTests
{
    public class UpdateOrderStatusTest
    {
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly Mock<ILogger<OrdersService>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ICompanyService> _companyServiceMock;
        private readonly Mock<IUserService> _userServiceMock;

        public UpdateOrderStatusTest()
        {
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger<OrdersService>>();
            _configurationMock = new Mock<IConfiguration>();
            _companyServiceMock = new Mock<ICompanyService>();
            _userServiceMock = new Mock<IUserService>();
        }

        [Fact]
        public async Task UpdateOrderStatus_SuccessfulRequest()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var orderStatus = new OrderStatusDTO
            {
                OrderId = "fakeOrderId",
                OrderStatus = OrderStatusEnum.Dispatched
            };
            var expectedBaseAddress = "https://thamco_orders_azureexample.net/api/";
            var expectedEndpoint = "orders/UpdateOrderStatus";

            _configurationMock.SetupGet(c => c["Services:Order:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:Order:UpdateOrderStatusEndpoint"]).Returns(expectedEndpoint);

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

            var orderService = new OrdersService(
                _clientFactoryMock.Object,
                _loggerMock.Object,
                _configurationMock.Object,
                _userServiceMock.Object,
                _companyServiceMock.Object);

            // Act
            var result = await orderService.UpdateOrderStatus(accessToken, orderStatus);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateOrderStatus_UnsuccessfulRequest()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var orderStatus = new OrderStatusDTO
            {
                OrderId = "fakeOrderId",
                OrderStatus = OrderStatusEnum.Dispatched
            };
            var expectedBaseAddress = "https://thamco_orders_azureexample.net/api/";
            var expectedEndpoint = "orders/UpdateOrderStatus";

            _configurationMock.SetupGet(c => c["Services:Order:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:Order:UpdateOrderStatusEndpoint"]).Returns(expectedEndpoint);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest // Simulate an unsuccessful request
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(expectedBaseAddress)
            };

            _clientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var orderService = new OrdersService(
                _clientFactoryMock.Object,
                _loggerMock.Object,
                _configurationMock.Object,
                _userServiceMock.Object,
                _companyServiceMock.Object);

            // Act
            var result = await orderService.UpdateOrderStatus(accessToken, orderStatus);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateOrderStatus_HandlesExceptionOnRequest()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var orderStatus = new OrderStatusDTO
            {
                OrderId = "fakeOrderId",
                OrderStatus = OrderStatusEnum.Dispatched
            };
            var expectedException = new HttpRequestException("Simulated error");
            var expectedBaseAddress = "https://thamco_orders_azureexample.net/api/";
            var expectedEndpoint = "orders/UpdateOrderStatus";

            _configurationMock.SetupGet(c => c["Services:Order:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:Order:UpdateOrderStatusEndpoint"]).Returns(expectedEndpoint);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(expectedException);

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(expectedBaseAddress)
            };

            _clientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var orderService = new OrdersService(
                _clientFactoryMock.Object,
                _loggerMock.Object,
                _configurationMock.Object,
                _userServiceMock.Object,
                _companyServiceMock.Object);

            // Act
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => orderService.GetOrdersCount(accessToken));

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Simulated error", exception.Message);
        }
    }
}
