using DTOs;
using DTOs.Customers;
using DTOs.Orders;
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
    public class AddNewOrderTest
    {
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly Mock<ILogger<OrdersService>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ICompanyService> _companyServiceMock;
        private readonly Mock<IUserService> _userServiceMock;

        public AddNewOrderTest()
        {
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger<OrdersService>>();
            _configurationMock = new Mock<IConfiguration>();
            _companyServiceMock = new Mock<ICompanyService>();
            _userServiceMock = new Mock<IUserService>();
        }

        [Fact]
        public async Task AddNewOrder_SuccessfulRequest()
        {
            // Arrange
            var orderAccessToken = "orderAccessToken";
            var usersAccessToken = "usersAccessToken";
            var addNewOrderDTO = new AddNewOrderDTO
            {
                OrderCreationDate = "2023-01-01",
                CreatedBy = "JohnDoe",
                OrderNotes = "Some order notes",
                Subtotal = "50.00",
                DeliveryCharge = "5.00",
                Total = "55.00",
                CustomerId = "12345",
                Customer = new CustomerDTO
                {
                    CustomerId = "12345",
                    CustomerName = "JohnDoe",
                    // ... other properties
                },
                Address = new AddressDTO
                {
                    Street = "123 Main St",
                    City = "Cityville",
                    country = "Stateville",
                    PostalCode = "12345",
                    // ... other properties
                },
                BillingAddress = new CompanyDetailsDTO
                {
                    CompanyAddressId = "12345",
                    CompanyName = "Example Company",
                    ShopNumber = "A123",
                    Street = "Main Street",
                    City = "Example City",
                    Country = "Example Country",
                    PostalCode = "12345"
                    // ... other properties
                },
                Status = OrderStatusEnum.Waiting,
                OrderedItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        ProductId = 1,
                        ProductName = "Product1",
                        TotalQuantity = 2,
                        TotalPrice = 20.00,
                        // ... other properties
                    },
                }
            };


            var expectedBaseAddress = "https://thamco_orders_azureexample.net/api/";
            var expectedEndpoint = "orders/AddNewOrder";

            _configurationMock.SetupGet(c => c["Services:Order:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:Order:AddNewOrderEndpoint"]).Returns(expectedEndpoint);

            _companyServiceMock.Setup(c => c.GetCompanyDetails()).Returns(new CompanyDetailsDTO
            {
                CompanyAddressId = "12345",
                CompanyName = "Example Company",
                ShopNumber = "A123",
                Street = "Main Street",
                City = "Example City",
                Country = "Example Country",
                PostalCode = "12345"
            });

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

            _userServiceMock.Setup(u => u.DirectAlterCustomersFunds(usersAccessToken, It.IsAny<CustomerFundsDTO>()))
                .Verifiable();

            var orderService = new OrdersService(
                _clientFactoryMock.Object,
                _loggerMock.Object,
                _configurationMock.Object,
                _userServiceMock.Object,
                _companyServiceMock.Object);

            // Act
            var result = await orderService.AddNewOrder(orderAccessToken, usersAccessToken, addNewOrderDTO);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AddNewOrder_UnsuccessfulRequest()
        {
            // Arrange
            var orderAccessToken = "orderAccessToken";
            var usersAccessToken = "usersAccessToken";
            var addNewOrderDTO = new AddNewOrderDTO
            {
                OrderCreationDate = "2023-01-01",
                CreatedBy = "JohnDoe",
                OrderNotes = "Some order notes",
                Subtotal = "50.00",
                DeliveryCharge = "5.00",
                Total = "55.00",
                CustomerId = "12345",
                Customer = new CustomerDTO
                {
                    CustomerId = "12345",
                    CustomerName = "JohnDoe",
                    // ... other properties
                },
                Address = new AddressDTO
                {
                    Street = "123 Main St",
                    City = "Cityville",
                    country = "Stateville",
                    PostalCode = "12345",
                    // ... other properties
                },
                BillingAddress = new CompanyDetailsDTO
                {
                    CompanyAddressId = "12345",
                    CompanyName = "Example Company",
                    ShopNumber = "A123",
                    Street = "Main Street",
                    City = "Example City",
                    Country = "Example Country",
                    PostalCode = "12345"
                    // ... other properties
                },
                Status = OrderStatusEnum.Waiting,
                OrderedItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        ProductId = 1,
                        ProductName = "Product1",
                        TotalQuantity = 2,
                        TotalPrice = 20.00,
                        // ... other properties
                    },
                }
            };

            var expectedException = new HttpRequestException("Simulated error");
            var expectedBaseAddress = "https://thamco_orders_azureexample.net/api/";
            var expectedEndpoint = "orders/AddNewOrder";

            _configurationMock.SetupGet(c => c["Services:Order:BaseAddress"]).Returns(expectedBaseAddress);
            _configurationMock.SetupGet(c => c["Services:Order:AddNewOrderEndpoint"]).Returns(expectedEndpoint);

            _companyServiceMock.Setup(c => c.GetCompanyDetails()).Returns(new CompanyDetailsDTO
            {
                CompanyAddressId = "12345",
                CompanyName = "Example Company",
                ShopNumber = "A123",
                Street = "Main Street",
                City = "Example City",
                Country = "Example Country",
                PostalCode = "12345"
            });

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
            var result = await Assert.ThrowsAsync<HttpRequestException>(() => orderService.AddNewOrder(orderAccessToken, usersAccessToken, addNewOrderDTO));
            Assert.Equal(expectedException, result);
        }
    }
}
