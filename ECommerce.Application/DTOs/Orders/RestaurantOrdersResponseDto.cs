namespace ECommerce.Application.DTOs.Orders
{
    public class RestaurantOrdersResponseDto
    {
        public List<RestaurantOrderDto> NewOrders { get; set; } = new();
        public List<RestaurantOrderDto> Preparing { get; set; } = new();
        public List<RestaurantOrderDto> Ready { get; set; } = new();
        public List<RestaurantOrderDto> OutForDelivery { get; set; } = new();
        public List<RestaurantOrderDto> History { get; set; } = new();
    }
}
