using ECommerce.Application.DTOs.Carts;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartsController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carts = await _cartService.GetAllCartsAsync();

            return Ok(carts);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);

            if (cart is null) return NotFound();

            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCartDto request)
        {
            var cart = await _cartService.AddCartAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = cart.Id },
                cart);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] int userId)
        {
            var cart = await _cartService.UpdateCartAsync(id, userId);

            if (cart is null) return NotFound();

            return Ok(cart);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cart = await _cartService.DeleteCartAsync(id);

            if (cart is null) return NotFound();

            return Ok(cart);
        }
    }
}
