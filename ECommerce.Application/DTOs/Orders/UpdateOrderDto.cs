using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.Orders
{
    public class UpdateOrderDto
    {
        public int? UserId { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
