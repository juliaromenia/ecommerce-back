using ECommerce.Application.DTOs.DeliveryAddress;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.DTOs.OrdersItens;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                CreatedAt = o.CreatedAt
            });
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt
            };
        }

        public async Task<OrderDto> AddOrderAsync(CreateOrderDto orderDto)
        {
            var order = new Domain.Entities.Order
            {
                UserId = orderDto.UserId,
                Status = orderDto.Status,
                TotalAmount = orderDto.TotalAmount,
                CreatedAt = DateTime.UtcNow
            };

            await _orderRepository.AddAsync(order);

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt
            };
        }

        public async Task<OrderDto?> UpdateOrderAsync(int id, UpdateOrderDto orderDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null) throw new Exception("Order not found");

            if (orderDto.UserId.HasValue)
                order.UserId = orderDto.UserId.Value;

            if (orderDto.Status.HasValue)
                order.Status = orderDto.Status.Value;

            if (orderDto.TotalAmount.HasValue)
                order.TotalAmount = orderDto.TotalAmount.Value;

            if (orderDto.CreatedAt.HasValue)
                order.CreatedAt = orderDto.CreatedAt.Value;

            await _orderRepository.UpdateAsync(order);

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt
            };
        }

        public async Task<string?> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null) throw new Exception("Order não encontrado");

            await _orderRepository.DeleteAsync(order);

            return "Order deletado com sucesso";
        }

        public async Task<RestaurantOrdersResponseDto> GetRestaurantOrdersAsync(DateTime date)
        {
            var orders = await _orderRepository.GetAllAsync();

            var ordersOfDay = orders
                .Where(o => o.CreatedAt.Date == date.Date)
                .ToList();

            var response = new RestaurantOrdersResponseDto
            {
                NewOrders = ordersOfDay
                    .Where(o => o.Status == OrderStatus.Pending)
                    .Select(MapToDto)
                    .ToList(),

                Preparing = ordersOfDay
                    .Where(o => o.Status == OrderStatus.Preparing)
                    .Select(MapToDto)
                    .ToList(),

                Ready = ordersOfDay
                    .Where(o => o.Status == OrderStatus.Ready)
                    .Select(MapToDto)
                    .ToList(),

                OutForDelivery = ordersOfDay
                    .Where(o => o.Status == OrderStatus.OutForDelivery)
                    .Select(MapToDto)
                    .ToList(),

                History = ordersOfDay
                    .Where(o => 
                        o.Status == OrderStatus.Delivered 
                        || o.Status == OrderStatus.Cancelled)
                    .Select(MapToDto)
                    .ToList(),
            };

            return response;
        }

        private static RestaurantOrderDto MapToDto(Order order)
        {
            return new RestaurantOrderDto
            {
                Id = order.Id,
                OrderNumber = order.Id,
                Status = order.Status,
                CustomerName = order.User.Name,
                CreatedAt = order.CreatedAt,
                DeliveryFee = order.DeliveryFee,
                TotalAmount = order.TotalAmount,
                DeliveryPersonName = order.DeliveryPerson?.Name,

                DeliveryAddress = new DeliveryAddressDto
                {
                    Street = order.DeliveryStreet,
                    Number = order.DeliveryNumber,
                    Complement = order.DeliveryComplement,
                    Neighborhood = order.DeliveryNeighborhood,
                    City = order.DeliveryCity,
                    State = order.DeliveryState,
                    ZipCode = order.DeliveryZipCode
                },

                Items = order.OrderItems
                    .Select(item => new OrderItemDto
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    })
                    .ToList()
            };
        }
    }
}
