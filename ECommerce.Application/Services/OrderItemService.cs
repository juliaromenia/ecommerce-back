using ECommerce.Application.DTOs.OrdersItens;
using ECommerce.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class OrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllOrderItemsAsync()
        {
            var orderItems = await _orderItemRepository.GetAllAsync();
            return orderItems.Select(oi => new OrderItemDto
            {
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            });
        }

        public async Task<OrderItemDto?> GetOrderItemByIdAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null) return null;
            return new OrderItemDto
            {
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice
            };
        }

        public async Task<OrderItemDto> AddOrderItemAsync(CreateOrderItemDto createOrderItemDto)
        {
            var orderItem = new Domain.Entities.OrderItem
            {
                OrderId = createOrderItemDto.OrderId,
                ProductId = createOrderItemDto.ProductId,
                Quantity = createOrderItemDto.Quantity,
                UnitPrice = createOrderItemDto.UnitPrice
            };

            await _orderItemRepository.AddAsync(orderItem);

            return new OrderItemDto
            {
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice
            };
        }

        public async Task<OrderItemDto?> UpdateOrderItemAsync(int id, UpdateOrderItemDto updateOrderItemDto)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);

            if (orderItem == null) return null;

            if(updateOrderItemDto.OrderId.HasValue)
                orderItem.OrderId = updateOrderItemDto.OrderId.Value;

            if(updateOrderItemDto.ProductId.HasValue)
                orderItem.ProductId = updateOrderItemDto.ProductId.Value;

            if(updateOrderItemDto.Quantity.HasValue)
                orderItem.Quantity = updateOrderItemDto.Quantity.Value;

            if(updateOrderItemDto.UnitPrice.HasValue)
                orderItem.UnitPrice = updateOrderItemDto.UnitPrice.Value;

            await _orderItemRepository.UpdateAsync(orderItem);

            return new OrderItemDto
            {
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice
            };
        }

        public async Task<string?> DeleteOrderItemAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);

            if (orderItem == null) throw new Exception($"OrderItem with id {id} not found.");

            await _orderItemRepository.DeleteAsync(orderItem);

            return $"OrderItem with id {id} has been deleted successfully.";
        }
    }
}
