using ECommerce.Application.DTOs.Carts;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Services
{
    public class CartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IEnumerable<CartDto>> GetAllCartsAsync()
        {
            var carts = await _cartRepository.GetAllAsync();

            return carts.Select(c => new CartDto
            {
                Id = c.Id,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt
            });
        }

        public async Task<CartDto?> GetCartByIdAsync(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null) return null;

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt
            };
        }

        public async Task<CartDto> AddCartAsync(CreateCartDto cartDto)
        {
            var cart = new Domain.Entities.Cart
            {
                UserId = cartDto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _cartRepository.AddAsync(cart);

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt
            };
        }

        public async Task<CartDto> UpdateCartAsync(int id, int userId)
        {
            var cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null) throw new Exception("Cart not found");

            cart.UserId = userId;

            await _cartRepository.UpdateAsync(cart);

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt
            };
        }

        public async Task<string?> DeleteCartAsync(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null) throw new Exception("Cart não encontrado");

            await _cartRepository.DeleteAsync(cart);

            return "Carrinho esvazeado com sucesso!";
        }
    }
}
