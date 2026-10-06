using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
    }
}
