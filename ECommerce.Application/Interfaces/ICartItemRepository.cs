using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface ICartItemRepository
    {
        Task<IEnumerable<CartItem>> GetAllAsync();
        Task<CartItem?> GetByIdAsync(int id);
        Task<CartItem?> GetByCartAndProductAsync(int cartId, int productId);
        Task AddAsync(CartItem cartItem);
        Task UpdateAsync(CartItem cartItem);
        Task DeleteAsync(CartItem cartItem);
    }
}
