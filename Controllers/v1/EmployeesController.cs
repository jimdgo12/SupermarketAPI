using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using SupermarketAPI.Dtos;

namespace SupermarketAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // 🔹 POST → Crear empleado
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest(new ActionMessageDto { Message = "El cuerpo de la solicitud no contiene un empleado válido." });

            try
            {
                int newId = await _employeeService.CreateEmployeeAsync(employee);
                _logger.LogInformation("Empleado creado con ID {Id}", newId);

                var response = new EmployeeResponseDto
                {
                    Id = newId,
                    IdentificationNumber = employee.IdentificationNumber,
                    FirstName = employee.FirstName,
                    MiddleName = employee.MiddleName,
                    LastName1 = employee.LastName1,
                    LastName2 = employee.LastName2,
                    RoleId = employee.RoleId,
                    BranchId = employee.BranchId,
                    Message = "El empleado fue creado de manera exitosa."
                };

                return CreatedAtAction(nameof(GetById), new { id = newId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el empleado.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al crear el empleado." });
            }
        }

        // 🔹 GET → Obtener todos los empleados
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();

                if (employees == null || !employees.Any())
                {
                    return Ok(new { message = "No hay empleados registrados.", data = new List<Employee>() });
                }

                var response = employees.Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    IdentificationNumber = e.IdentificationNumber,
                    FirstName = e.FirstName,
                    MiddleName = e.MiddleName,
                    LastName1 = e.LastName1,
                    LastName2 = e.LastName2,
                    RoleId = e.RoleId,
                    BranchId = e.BranchId,
                    Message = "Empleado cargado correctamente."
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los empleados.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener los empleados." });
            }
        }

        // 🔹 GET → Obtener empleado por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el empleado con ID {id}." });

                var response = new EmployeeResponseDto
                {
                    Id = employee.Id,
                    IdentificationNumber = employee.IdentificationNumber,
                    FirstName = employee.FirstName,
                    MiddleName = employee.MiddleName,
                    LastName1 = employee.LastName1,
                    LastName2 = employee.LastName2,
                    RoleId = employee.RoleId,
                    BranchId = employee.BranchId,
                    Message = "Empleado encontrado."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el empleado con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener el empleado." });
            }
        }

        // 🔹 PUT → Actualizar empleado
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Employee employee)
        {
            if (employee == null || employee.Id != id)
                return BadRequest(new ActionMessageDto { Message = "El empleado es nulo o el ID no coincide." });

            try
            {
                bool updated = await _employeeService.UpdateEmployeeAsync(employee);
                if (!updated)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el empleado con ID {id} para actualizar." });

                var response = new EmployeeResponseDto
                {
                    Id = employee.Id,
                    IdentificationNumber = employee.IdentificationNumber,
                    FirstName = employee.FirstName,
                    MiddleName = employee.MiddleName,
                    LastName1 = employee.LastName1,
                    LastName2 = employee.LastName2,
                    RoleId = employee.RoleId,
                    BranchId = employee.BranchId,
                    Message = "El empleado fue actualizado de manera exitosa."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el empleado con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al actualizar el empleado." });
            }
        }

        // 🔹 DELETE → Eliminar empleado
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool deleted = await _employeeService.DeleteEmployeeAsync(id);
                if (!deleted)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el empleado con ID {id} para eliminar." });

                return Ok(new ActionMessageDto { Message = "El empleado fue eliminado de manera exitosa." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el empleado con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al eliminar el empleado." });
            }
        }
    }
}
