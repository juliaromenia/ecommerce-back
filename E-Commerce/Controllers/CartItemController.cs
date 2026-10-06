using ECommerce.Application.DTOs.CartsItens;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : ControllerBase
    {
        private readonly CartItemService _cartItemService;

        public CartItemController(CartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cartItems = await _cartItemService.GetAllCartItemsAsync();
            return Ok(cartItems);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cartItem = await _cartItemService.GetCartItemByIdAsync(id);
            if (cartItem is null) return NotFound();
            return Ok(cartItem);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCartItemDto cartItemDto)
        {
            var createdCartItem = await _cartItemService.AddCartItemAsync(cartItemDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCartItem.Id }, createdCartItem);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, int quantity)
        {
            var updatedCartItem = await _cartItemService
                .UpdateCartItemAsync(id, quantity);

            if (updatedCartItem is null)
                return NotFound();

            return Ok(updatedCartItem);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedCartItem = await _cartItemService.DeleteCartItemAsync(id);
            if (deletedCartItem is null) return NotFound();
            return Ok(deletedCartItem);
        }
    }
}
