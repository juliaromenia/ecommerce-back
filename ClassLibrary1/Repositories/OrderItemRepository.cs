using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ECommerceDbContext _context;
        public OrderItemRepository(ECommerceDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItems
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _context.OrderItems
                .AsNoTracking()
                .FirstOrDefaultAsync(oi => oi.Id == id);
        }
        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsInActiveOrderAsync(int productId)
        {
            return await (
                from orderItem in _context.OrderItems
                join order in _context.Orders
                    on orderItem.OrderId equals order.Id
                where orderItem.ProductId == productId
                      && order.Status != OrderStatus.Delivered
                      && order.Status != OrderStatus.Cancelled
                select orderItem
            ).AnyAsync();
        }
    }
}
