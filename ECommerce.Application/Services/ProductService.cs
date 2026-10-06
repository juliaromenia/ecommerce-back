using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public ProductService(IProductRepository productRepository, IOrderItemRepository orderItemRepository)
        {
            _productRepository = productRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                IsAvailable = p.IsAvailable,
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                IsAvailable = product.IsAvailable,
            };
        }

        public async Task<PagedResult<ProductDto>> GetProductsAsync(ProductFilterRequest request)
        {
            if (request.Page < 1)
                request.Page = 1;

            if (request.PageSize < 1)
                request.PageSize = 15;

            var products = await _productRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                products = products.Where(p => p.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (request.CategoryId.HasValue)
            {
                products = products.Where(p =>
                    p.CategoryId == request.CategoryId.Value);
            }

            var totalItems = products.Count();

            var pagedProducts = products
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            var productDtos = pagedProducts.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                IsAvailable = product.IsAvailable,
            }).OrderByDescending(products => products.IsAvailable);

            var totalPages = (int)Math.Ceiling((double)totalItems / request.PageSize);

            return new PagedResult<ProductDto>
            {
                Items = productDtos,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductRequestDto request)
        {
            if (request.Price <= 0)
                throw new ArgumentException("O preço deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("O nome do produto não pode estar vazio.");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow,
                IsAvailable = request.IsAvailable,
            };

            await _productRepository.AddAsync(product);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                IsAvailable = product.IsAvailable,
            };
        }

        public async Task<ProductDto?> UpdateAsync(int id, UpdateProductRequestDto request)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null) return null;

            if (request.Name is not null)
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    throw new ArgumentException("O nome do produto é obrigatório.");

                product.Name = request.Name;
            }

            if (request.Description is not null)
            {
                product.Description = request.Description;
            }

            if (request.Price.HasValue)
            {
                if (request.Price <= 0)
                    throw new ArgumentException(
                        "O preço do produto deve ser maior que zero.");

                product.Price = request.Price.Value;
            }

            if (request.CategoryId.HasValue)
            {
                product.CategoryId = request.CategoryId.Value;
            }

            if (request.IsAvailable is not null)
            {
                product.IsAvailable = request.IsAvailable.Value;
            }

            await _productRepository.UpdateAsync(product);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                IsAvailable = product.IsAvailable,
            };
        }

        //TODO: fazer a validação se existe pedidos em andamento antes de deletar o produto
        public async Task<string?> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null) return null;

            var existsInActiveOrder = await _orderItemRepository.ExistsInActiveOrderAsync(id);

            if (existsInActiveOrder)
            {
                throw new InvalidOperationException("Não é possível deletar o produto, pois ele está associado a pedidos em andamento.");
            }

            await _productRepository.DeleteAsync(product);

            return "Produto deletado com sucesso!";
        }

        public async Task<IEnumerable<MostSoldProductDto>> GetTopSoldProductsAsync(int year, int month)
        {
            return await _productRepository
                .GetTopSoldProductsAsync(year, month);
        }
    }
}
