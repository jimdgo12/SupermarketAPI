using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using SupermarketAPI.Dtos;

namespace SupermarketAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly ILogger<BranchesController> _logger;

        public BranchesController(IBranchService branchService, ILogger<BranchesController> logger)
        {
            _branchService = branchService ?? throw new ArgumentNullException(nameof(branchService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // POST → Crear sucursal
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Branch branch)
        {
            _logger.LogInformation("POST /api/v1/branches recibido.");

            if (branch == null)
                return BadRequest(new ActionMessageDto { Message = "El cuerpo de la solicitud no contiene una sucursal válida." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int newId = await _branchService.CreateBranchAsync(branch);
                _logger.LogInformation("Sucursal creada con ID {Id}", newId);

                var response = new BranchResponseDto
                {
                    Id = newId,
                    Nit = branch.Nit,
                    Name = branch.Name,
                    Address = branch.Address,
                    Phone = branch.Phone,
                    Email = branch.Email,
                    City = branch.City,
                    Message = "La sucursal fue creada de manera exitosa."
                };

                return CreatedAtAction(nameof(GetById), new { id = newId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la sucursal.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al crear la sucursal." });
            }
        }

        // GET → Obtener todas las sucursales
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/v1/branches recibido.");

            try
            {
                var branches = await _branchService.GetAllBranchesAsync();

                if (branches == null || !branches.Any())
                {
                    return Ok(new
                    {
                        message = "No hay sucursales registradas.",
                        data = new List<Branch>()
                    });
                }

                var response = branches.Select(b => new BranchResponseDto
                {
                    Id = b.Id,
                    Nit = b.Nit,
                    Name = b.Name,
                    Address = b.Address,
                    Phone = b.Phone,
                    Email = b.Email,
                    City = b.City,
                    Message = "Sucursal cargada correctamente."
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las sucursales.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener las sucursales." });
            }
        }

        // GET → Obtener sucursal por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GET /api/v1/branches/{Id} recibido.", id);

            try
            {
                var branch = await _branchService.GetBranchByIdAsync(id);
                if (branch == null)
                {
                    _logger.LogWarning("⚠️ Sucursal con ID {Id} no encontrada.", id);
                    return NotFound(new ActionMessageDto { Message = $"No se encontró la sucursal con ID {id}." });
                }

                var response = new BranchResponseDto
                {
                    Id = branch.Id,
                    Nit = branch.Nit,
                    Name = branch.Name,
                    Address = branch.Address,
                    Phone = branch.Phone,
                    Email = branch.Email,
                    City = branch.City,
                    Message = "Sucursal encontrada."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la sucursal con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener la sucursal." });
            }
        }

        // PUT → Actualizar sucursal
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Branch branch)
        {
            _logger.LogInformation("PUT /api/v1/branches/{Id} recibido.", id);

            if (branch == null || branch.Id != id)
                return BadRequest(new ActionMessageDto { Message = "La sucursal es nula o el ID no coincide." });

            try
            {
                bool updated = await _branchService.UpdateBranchAsync(branch);
                if (!updated)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró la sucursal con ID {id} para actualizar." });

                var response = new BranchResponseDto
                {
                    Id = branch.Id,
                    Nit = branch.Nit,
                    Name = branch.Name,
                    Address = branch.Address,
                    Phone = branch.Phone,
                    Email = branch.Email,
                    City = branch.City,
                    Message = "La sucursal fue actualizada de manera exitosa."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la sucursal con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al actualizar la sucursal." });
            }
        }

        // DELETE → Eliminar sucursal
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /api/v1/branches/{Id} recibido.", id);

            try
            {
                bool deleted = await _branchService.DeleteBranchAsync(id);
                if (!deleted)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró la sucursal con ID {id} para eliminar." });

                return Ok(new ActionMessageDto { Message = "La sucursal fue eliminada de manera exitosa." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la sucursal con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al eliminar la sucursal." });
            }
        }
    }
}
