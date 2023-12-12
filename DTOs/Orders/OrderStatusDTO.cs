using Enums;

namespace DTOs.Orders
{
    public class OrderStatusDTO
    {
        public string OrderId { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
    }
}
