using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductFilterRequest request)
        {
            var products = await _productService.GetProductsAsync(request);

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            
            if (product is null) return NotFound();
            
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequestDto request)
        {   
            var product = await _productService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById), 
                new { id = product.Id }, 
                product);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateProductRequestDto request)
        {
            var product = await _productService.UpdateAsync(id, request);

            if (product is null) return NotFound();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.DeleteAsync(id);

            if (product is null) return NotFound();

            return Ok(product);
        }

        [HttpGet("top-sold")]
        public async Task<ActionResult<IEnumerable<MostSoldProductDto>>> GetTopSoldProducts(
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var products = await _productService
                .GetTopSoldProductsAsync(year, month);

            return Ok(products);
        }
    }
}
