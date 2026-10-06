using ECommerce.Application.DTOs.CartsItens;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class CartItemService
    {
        private readonly ICartItemRepository _cartItemRepository; IProductRepository _productRepository; ICartRepository _cartRepository;

        public CartItemService(ICartItemRepository cartItemRepository, IProductRepository productRepository, ICartRepository cartRepository)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public async Task<IEnumerable<CartItemDto>> GetAllCartItemsAsync()
        {
            var cartItem = await _cartItemRepository.GetAllAsync();

            return cartItem.Select(c => new CartItemDto
            {
                Id = c.Id,
                CartId = c.CartId,
                ProductId = c.ProductId,
                Quantity = c.Quantity
            });
        }

        public async Task<CartItemDto?> GetCartItemByIdAsync(int id)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(id);

            if (cartItem == null) return null;

            return new CartItemDto
            {
                Id = cartItem.Id,
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };
        }

        public async Task<CartItemDto> AddCartItemAsync(CreateCartItemDto cartItemDto)
        {
            if (cartItemDto.Quantity <= 0)
                throw new ArgumentException(
                    "Quantity must be greater than zero.");

            var cart = await _cartRepository
                .GetByIdAsync(cartItemDto.CartId);

            if (cart is null)
                throw new ArgumentException(
                    "Cart not found.");

            var product = await _productRepository
                .GetByIdAsync(cartItemDto.ProductId);

            if (product is null)
                throw new ArgumentException(
                    "Product not found.");

            var existingItem = await _cartItemRepository
                .GetByCartAndProductAsync(
                    cartItemDto.CartId,
                    cartItemDto.ProductId);

            if (existingItem is not null)
            {
                existingItem.Quantity += cartItemDto.Quantity;

                await _cartItemRepository.UpdateAsync(existingItem);

                return new CartItemDto
                {
                    Id = existingItem.Id,
                    CartId = existingItem.CartId,
                    ProductId = existingItem.ProductId,
                    Quantity = existingItem.Quantity
                };
            }

            var cartItem = new CartItem
            {
                CartId = cartItemDto.CartId,
                ProductId = cartItemDto.ProductId,
                Quantity = cartItemDto.Quantity
            };

            await _cartItemRepository.AddAsync(cartItem);

            return new CartItemDto
            {
                Id = cartItem.Id,
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };
        }

        public async Task<CartItemDto?> UpdateCartItemAsync(
            int cartItemId,
            int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException(
                    "Quantity must be greater than zero.");

            var cartItem = await _cartItemRepository
                .GetByIdAsync(cartItemId);

            if (cartItem is null)
                throw new ArgumentException(
                    "Cart item not found.");

            var product = await _productRepository
                .GetByIdAsync(cartItem.ProductId);

            if (product is null)
                throw new ArgumentException(
                    "Product not found.");

            cartItem.Quantity = quantity;

            await _cartItemRepository.UpdateAsync(cartItem);

            return new CartItemDto
            {
                Id = cartItem.Id,
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };
        }

        public async Task<string> DeleteCartItemAsync(int id)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(id);

            if (cartItem is null)
                return "Erro ao deletar";

            await _cartItemRepository.DeleteAsync(cartItem);

            return "Deletado com sucesso";
        }
    }
}
