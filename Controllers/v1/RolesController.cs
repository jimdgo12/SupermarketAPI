using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using SupermarketAPI.Dtos;

namespace SupermarketAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // 🔹 POST → Crear rol
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Role role)
        {
            _logger.LogInformation("POST /api/v1/roles recibido.");

            if (role == null)
                return BadRequest(new ActionMessageDto { Message = "El cuerpo de la solicitud no contiene un rol válido." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int newId = await _roleService.CreateRoleAsync(role);
                _logger.LogInformation("Rol creado con ID {Id}", newId);

                var response = new RoleResponseDto
                {
                    Id = newId,
                    Name = role.Name,
                    Description = role.Description,
                    Message = "El rol fue creado de manera exitosa."
                };

                return CreatedAtAction(nameof(GetById), new { id = newId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el rol.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al crear el rol." });
            }
        }

        // 🔹 GET → Obtener todos los roles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/v1/roles recibido.");

            try
            {
                var roles = await _roleService.GetAllRolesAsync();

                if (roles == null || !roles.Any())
                {
                    return Ok(new
                    {
                        message = "No hay roles registrados.",
                        data = new List<Role>()
                    });
                }

                var response = roles.Select(r => new RoleResponseDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Message = "Rol cargado correctamente."
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener los roles." });
            }
        }

        // 🔹 GET → Obtener rol por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GET /api/v1/roles/{Id} recibido.", id);

            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    _logger.LogWarning("⚠️ Rol con ID {Id} no encontrado.", id);
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el rol con ID {id}." });
                }

                var response = new RoleResponseDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    Message = "Rol encontrado."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener el rol." });
            }
        }

        // 🔹 PUT → Actualizar rol
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Role role)
        {
            _logger.LogInformation("PUT /api/v1/roles/{Id} recibido.", id);

            if (role == null || role.Id != id)
                return BadRequest(new ActionMessageDto { Message = "El rol es nulo o el ID no coincide." });

            try
            {
                bool updated = await _roleService.UpdateRoleAsync(role);
                if (!updated)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el rol con ID {id} para actualizar." });

                var response = new RoleResponseDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    Message = "El rol fue actualizado de manera exitosa."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al actualizar el rol." });
            }
        }

        // 🔹 DELETE → Eliminar rol
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /api/v1/roles/{Id} recibido.", id);

            try
            {
                bool deleted = await _roleService.DeleteRoleAsync(id);
                if (!deleted)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el rol con ID {id} para eliminar." });

                return Ok(new ActionMessageDto { Message = "El rol fue eliminado de manera exitosa." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al eliminar el rol." });
            }
        }
    }
}
