using DTOs.Orders;

namespace Service.Interfaces.Orders
{
    public interface IOrdersService
    {
        public Task<bool> AddNewOrder(string? orderAccessToken, string? usersAccessToken, AddNewOrderDTO addNewOrderDTO);
    }
}
