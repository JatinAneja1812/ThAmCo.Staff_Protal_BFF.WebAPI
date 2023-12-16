using DTOs;
using DTOs.Orders;
using Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Service.Classes.Orders;
using Service.Interfaces.Company;
using Service.Interfaces.Orders;
using Service.Interfaces.Users;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace OrdersServiceTests
{
    public class GetAllOrdersTest
    {
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly Mock<ILogger<OrdersService>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ICompanyService> _companyServiceMock;
        private readonly Mock<IUserService> _userServiceMock;

        public GetAllOrdersTest()
        {
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger<OrdersService>>();
            _configurationMock = new Mock<IConfiguration>();
            _companyServiceMock = new Mock<ICompanyService>();
            _userServiceMock = new Mock<IUserService>();
        }

        [Fact]
        public async Task GetAllOrders_SuccessfulRequest()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var expectedOrders = GetFakeOrders(4);
            var expectedJson = JsonContent.Create(expectedOrders);
            var expectedBaseAddress = "https://thamco_orders_azureexample.net/api/";
            var expectedEndpoint = "orders/GetAllOrders";

            _configurationMock.SetupGet(c => c["Services:Order:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:Order:GetAllOrdersEndpoint"]).Returns(expectedEndpoint);

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

            var orderService = new OrdersService(
                _clientFactoryMock.Object,
                _loggerMock.Object,
                _configurationMock.Object,
                _userServiceMock.Object,
                _companyServiceMock.Object);

            // Act
            var result = await orderService.GetAllOrders(accessToken);

            // Assert
            Assert.Equal(expectedOrders.Count, result.Count);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllOrders_EmptyResponse()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var expectedJson = JsonContent.Create(new List<OrderDTO>()); // Empty response
            var expectedBaseAddress = "https://thamco_orders_azureexample.net/api/";
            var expectedEndpoint = "orders/GetAllOrders";

            _configurationMock.SetupGet(c => c["Services:Order:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:Order:GetAllOrdersEndpoint"]).Returns(expectedEndpoint);

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

            var orderService = new OrdersService(
                _clientFactoryMock.Object,
                _loggerMock.Object,
                _configurationMock.Object,
                _userServiceMock.Object,
                _companyServiceMock.Object);

            // Act
            var result = await orderService.GetAllOrders(accessToken);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllOrders_HandlesExceptionOnRequest()
        {
            // Arrange
            var accessToken = "dsfdhgdyu232beb32e32jbduy3byu3byud3bydu3b2ud32ud3udh3du3hdg2";
            var expectedException = new HttpRequestException("Simulated error");
            var expectedBaseAddress = "https://thamco_orders_azureexample.net/api/";
            var expectedEndpoint = "orders/GetAllOrders";

            _configurationMock.SetupGet(c => c["Services:Order:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:Order:GetAllOrdersEndpoint"]).Returns(expectedEndpoint);

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

            var orderService = new OrdersService(
                _clientFactoryMock.Object,
                _loggerMock.Object,
                _configurationMock.Object,
                _userServiceMock.Object,
                _companyServiceMock.Object);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => orderService.GetAllOrders(accessToken));

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Simulated error", exception.Message);
        }

        private List<OrderDTO> GetFakeOrders(int count)
        {
            var fakeOrders = new List<OrderDTO>();
            for (int i = 0; i < count; i++)
            {
                var order = new OrderDTO
                {
                    OrderId = Guid.NewGuid().ToString(),
                    OrderCreationDate = DateTime.Now.AddDays(-i),
                    DeliveryDate = DateTime.Now.AddDays(i),
                    CreatedBy = $"User{i}",
                    PaymentMethod = $"PaymentMethod{i}",
                    TotalPrice = 100.0 + i * 10,
                    Subtotal = 90.0 + i * 10,
                    DeliveryCharge = 10.0,
                    OrderNotes = $"OrderNotes{i}",
                    Customer = new CustomerDTO
                    {
                        CustomerId = Guid.NewGuid().ToString(),
                        CustomerName = $"User{i}",
                        CustomerEmailAddress = $"user{i}@example.com",
                        CustomerContactNumber = $"123-456-{i.ToString("D4")}",
                    },
                    Status = OrderStatusEnum.Created, // Set the order status as needed
                    OrderedItems = GetFakeOrderItems(3), // Adjust the count as needed
                    ShippingAddress = new AddressDTO
                    {
                        Street = $"ShippingStreet{i}",
                        City = $"ShippingCity{i}",
                        country = $"ShippingState{i}",
                        PostalCode = $"Shipping12345{i}",
                        HouseNumber = $"ShippingLOC{i}"
                    },
                    BillingAddress = new CompanyDetailsDTO
                    {
                        CompanyAddressId = Guid.NewGuid().ToString(),
                        CompanyName = $"Company Name",
                        ShopNumber = $"Company Number",
                        Street = $"Billing Street",
                        City = $"Billing City",
                        Country = "Billing Country",
                        PostalCode = $"Billing Postal Code NY32345"
                    }
                };

                fakeOrders.Add(order);
            }

            return fakeOrders;
        }

        // Helper method to generate fake order items for testing
        private ICollection<OrderItemDTO> GetFakeOrderItems(int count)
        {
            var fakeOrderItems = new List<OrderItemDTO>();
            for (int i = 0; i < count; i++)
            {
                var orderItem = new OrderItemDTO
                {
                    ProductId = i,
                    ProductName = $"Product{i}",
                    TotalQuantity = i + 1,
                    TotalPrice = 20.0 + i * 5
                };

                fakeOrderItems.Add(orderItem);
            }

            return fakeOrderItems;
        }
    }
}
