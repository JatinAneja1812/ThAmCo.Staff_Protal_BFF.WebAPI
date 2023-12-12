using DTOs.Customers;
using DTOs.Orders;
using DTOs.UserProfiles;

namespace Service.Interfaces.Orders
{
    public interface IOrdersService
    {
        public Task<List<OrderDTO>> GetAllOrders(string? accessToken);
        public Task<List<OrderDTO>> GetAllHistoricOrders(string? accessToken);
        public Task<int> GetOrdersCount(string? accessToken);
        public Task<bool> AddNewOrder(string? orderAccessToken, string? usersAccessToken, AddNewOrderDTO addNewOrderDTO);
        public Task<bool> RemoveOrder(string? orderAccessToken, string orderId);
        public Task<bool> UpdateOrderStatus(string? accessToken, OrderStatusDTO order);
        public Task<bool> UpdateOrderDeliveryDate(string? accessToken, ScheduledOrderDTO order);
    }
}
