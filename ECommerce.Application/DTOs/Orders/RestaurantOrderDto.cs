using ECommerce.Application.DTOs.DeliveryAddress;
using ECommerce.Application.DTOs.OrdersItens;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.Orders
{
    public class RestaurantOrderDto
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public OrderStatus Status { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? DeliveryPersonName { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal TotalAmount { get; set; }
        public DeliveryAddressDto DeliveryAddress { get; set; } = new();
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
