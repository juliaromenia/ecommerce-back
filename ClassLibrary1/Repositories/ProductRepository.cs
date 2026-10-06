using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .AsNoTracking() // adequado para operações de leitura, melhora o desempenho
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .AsNoTracking() //// adequado para operações de leitura, melhora o desempenho
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MostSoldProductDto>> GetTopSoldProductsAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            return await _context.OrderItems
                .Where(oi =>
                oi.Order.CreatedAt >= startDate &&
                oi.Order.CreatedAt < endDate &&
                oi.Order.Status != OrderStatus.Cancelled)
                .GroupBy(oi => new
                {
                    oi.ProductId,
                    oi.Product.Name,
                    oi.Product.Description,
                })
                .Select(g => new MostSoldProductDto
                {
                    Name = g.Key.Name,
                    QuantitySold = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(3)
                .ToListAsync();
        }
    }
}
