namespace ECommerce.Domain.Entities
{
    public class DeliveryPerson
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }

        public DateTime CreatedAt { get; set; }
    }   
}
