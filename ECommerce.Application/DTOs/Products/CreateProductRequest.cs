namespace ECommerce.Application.DTOs.Products
{
    public class CreateProductRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }  = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
