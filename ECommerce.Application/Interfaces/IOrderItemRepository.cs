using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<OrderItem?> GetByIdAsync(int id);
        Task AddAsync(OrderItem orderItem);
        Task UpdateAsync(OrderItem orderItem);
        Task DeleteAsync(OrderItem orderorderItem);
        Task<bool> ExistsInActiveOrderAsync(int productId);
    }
}
