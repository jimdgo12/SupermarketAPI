using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using SupermarketAPI.Dtos;

namespace SupermarketAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            _logger.LogInformation("POST /api/v1/products recibido.");

            if (product == null)
                return BadRequest(new ActionMessageDto { Message = "El cuerpo de la solicitud no contiene un producto válido." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int newId = await _productService.CreateProductAsync(product);
                _logger.LogInformation("Producto creado con ID {Id}", newId);

                var response = new ProductResponseDto
                {
                    Id = newId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    IsActive = product.IsActive,
                    Message = "El producto fue creado de manera exitosa."
                };

                return CreatedAtAction(nameof(GetById), new { id = newId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el producto.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al crear el producto." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/v1/products recibido.");

            try
            {
                var products = await _productService.GetAllProductsAsync();

                if (products == null || !products.Any())
                {
                    return Ok(new
                    {
                        message = "No hay productos registrados.",
                        data = new List<Product>()
                    });
                }

                var response = products.Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    IsActive = p.IsActive,
                    Message = "Producto cargado correctamente."
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los productos.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener los productos." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GET /api/v1/products/{Id} recibido.", id);

            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("⚠️ Producto con ID {Id} no encontrado.", id);
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el producto con ID {id}." });
                }

                var response = new ProductResponseDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    IsActive = product.IsActive,
                    Message = "Producto encontrado."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener el producto." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            _logger.LogInformation("PUT /api/v1/products/{Id} recibido.", id);

            if (product == null || product.Id != id)
                return BadRequest(new ActionMessageDto { Message = "El producto es nulo o el ID no coincide." });

            try
            {
                bool updated = await _productService.UpdateProductAsync(product);
                if (!updated)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el producto con ID {id} para actualizar." });

                var response = new ProductResponseDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    IsActive = product.IsActive,
                    Message = "El producto fue actualizado de manera exitosa."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el producto con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al actualizar el producto." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /api/v1/products/{Id} recibido.", id);

            try
            {
                bool deleted = await _productService.DeleteProductAsync(id);
                if (!deleted)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el producto con ID {id} para eliminar." });

                return Ok(new ActionMessageDto { Message = "El producto fue eliminado de manera exitosa." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el producto con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al eliminar el producto." });
            }
        }
    }
}
