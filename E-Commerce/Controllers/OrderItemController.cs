using ECommerce.Application.DTOs.OrdersItens;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly OrderItemService _orderItemService;

        public OrderItemController(OrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orderItems = await _orderItemService.GetAllOrderItemsAsync();

            return Ok(orderItems);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var orderItem = await _orderItemService.GetOrderItemByIdAsync(id);
            if (orderItem is null) return NotFound();
            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderItemDto orderItem)
        {
            var createdOrderItem = await _orderItemService.AddOrderItemAsync(orderItem);
            return CreatedAtAction(
                nameof(GetById),
                createdOrderItem);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderItemDto orderItemDto)
        {
            var updatedOrderItem = await _orderItemService.UpdateOrderItemAsync(id, orderItemDto);
            if (updatedOrderItem is null) return NotFound();
            return Ok(updatedOrderItem);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedOrderItem = await _orderItemService.DeleteOrderItemAsync(id);
            if (deletedOrderItem is null) return NotFound();
            return Ok(deletedOrderItem);
        }
    }
}
