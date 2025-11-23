using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using SupermarketAPI.Dtos;

namespace SupermarketAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            _logger.LogInformation("POST /api/v1/categories recibido.");

            if (category == null)
                return BadRequest(new ActionMessageDto { Message = "El cuerpo de la solicitud no contiene una categoría válida." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int newId = await _categoryService.CreateCategoryAsync(category);
                _logger.LogInformation("Categoría creada con ID {Id}", newId);

                var response = new CategoryResponseDto
                {
                    Id = newId,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive,
                    Message = "La categoría fue creada de manera exitosa."
                };

                return CreatedAtAction(nameof(GetById), new { id = newId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la categoría.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al crear la categoría." });
            }
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/v1/categories recibido.");

            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();

                if (categories == null || !categories.Any())
                {
                    return Ok(new
                    {
                        message = "No hay categorías registradas.",
                        data = new List<Category>()
                    });
                }

                var response = categories.Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive,
                    Message = "Categoría cargada correctamente."
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las categorías.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener las categorías." });
            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GET /api/v1/categories/{Id} recibido.", id);

            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("⚠️ Categoría con ID {Id} no encontrada.", id);
                    return NotFound(new ActionMessageDto { Message = $" No se encontró la categoría con ID {id}." });
                }

                var response = new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive,
                    Message = "Categoría encontrada."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la categoría con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener la categoría." });
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Category category)
        {
            _logger.LogInformation("PUT /api/v1/categories/{Id} recibido.", id);

            if (category == null || category.Id != id)
                return BadRequest(new ActionMessageDto { Message = "La categoría es nula o el ID no coincide." });

            try
            {
                bool updated = await _categoryService.UpdateCategoryAsync(category);
                if (!updated)
                    return NotFound(new ActionMessageDto { Message = $" No se encontró la categoría con ID {id} para actualizar." });

                var response = new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive,
                    Message = "La categoría fue actualizada de manera exitosa."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la categoría con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al actualizar la categoría." });
            }
        }


        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /api/v1/categories/{Id} recibido.", id);

            try
            {
                bool deleted = await _categoryService.DeleteCategoryAsync(id);
                if (!deleted)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró la categoría con ID {id} para eliminar." });

                return Ok(new ActionMessageDto { Message = "La categoría fue eliminada de manera exitosa." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la categoría con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al eliminar la categoría." });
            }
        }
    }
}
