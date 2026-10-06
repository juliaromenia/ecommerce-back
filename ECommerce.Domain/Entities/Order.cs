using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public decimal DeliveryFee { get; set; }

        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }

        // Controle de tempo
        public DateTime? PreparationStartedAt { get; set; }
        public DateTime? ReadyAt { get; set; }
        public DateTime? DeliveryStartedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        // Entrega
        public int? DeliveryPersonId { get; set; }
        public DeliveryPerson? DeliveryPerson { get; set; }

        // Endereço de entrega
        public string DeliveryStreet { get; set; } = string.Empty;
        public string DeliveryNumber { get; set; } = string.Empty;
        public string? DeliveryComplement { get; set; }
        public string DeliveryNeighborhood { get; set; } = string.Empty;
        public string DeliveryCity { get; set; } = string.Empty;
        public string DeliveryState { get; set; } = string.Empty;
        public string DeliveryZipCode { get; set; } = string.Empty;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
