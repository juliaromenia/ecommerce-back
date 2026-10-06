using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order is null) return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto order)
        {
            var createdOrder = await _orderService.AddOrderAsync(order);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdOrder.Id },
                createdOrder);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderDto orderDto)
        {
            var order = await _orderService.UpdateOrderAsync(id, orderDto);

            if (order is null) return NotFound();

            return Ok(order);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.DeleteOrderAsync(id);

            if (order is null) return NotFound();

            return Ok(order);
        }

        [HttpGet("restaurant")]
        public async Task<ActionResult<RestaurantOrdersResponseDto>> GetRestaurantOrders([FromQuery] DateTime date)
        {
            var oders = await _orderService.GetRestaurantOrdersAsync(date);

            return Ok(oders);
        }
    }
}

