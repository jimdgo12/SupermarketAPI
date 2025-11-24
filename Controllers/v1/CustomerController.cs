using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using SupermarketAPI.Dtos;

namespace SupermarketAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // POST /api/v1/customers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            _logger.LogInformation("POST /api/v1/customers recibido.");

            if (customer == null)
                return BadRequest(new ActionMessageDto { Message = "El cuerpo de la solicitud no contiene un cliente válido." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int newId = await _customerService.CreateCustomerAsync(customer);
                _logger.LogInformation("Cliente creado con ID {Id}", newId);

                var response = new CustomerResponseDto
                {
                    Id = newId,
                    IdentificationNumber = customer.IdentificationNumber,
                    FirstName = customer.FirstName,
                    MiddleName = customer.MiddleName,
                    LastName1 = customer.LastName1,
                    LastName2 = customer.LastName2,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    Message = "El cliente fue creado de manera exitosa."
                };

                return CreatedAtAction(nameof(GetById), new { id = newId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el cliente.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al crear el cliente." });
            }
        }

        // GET /api/v1/customers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/v1/customers recibido.");

            try
            {
                var customers = await _customerService.GetAllCustomersAsync();

                if (customers == null || !customers.Any())
                {
                    return Ok(new
                    {
                        message = "No hay clientes registrados.",
                        data = new List<Customer>()
                    });
                }

                var response = customers.Select(c => new CustomerResponseDto
                {
                    Id = c.Id,
                    IdentificationNumber = c.IdentificationNumber,
                    FirstName = c.FirstName,
                    MiddleName = c.MiddleName,
                    LastName1 = c.LastName1,
                    LastName2 = c.LastName2,
                    Email = c.Email,
                    Phone = c.Phone,
                    Address = c.Address,
                    Message = "Cliente cargado correctamente."
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los clientes.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener los clientes." });
            }
        }

        // GET /api/v1/customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GET /api/v1/customers/{Id} recibido.", id);

            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning("Cliente con ID {Id} no encontrado.", id);
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el cliente con ID {id}." });
                }

                var response = new CustomerResponseDto
                {
                    Id = customer.Id,
                    IdentificationNumber = customer.IdentificationNumber,
                    FirstName = customer.FirstName,
                    MiddleName = customer.MiddleName,
                    LastName1 = customer.LastName1,
                    LastName2 = customer.LastName2,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    Message = "Cliente encontrado."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el cliente con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener el cliente." });
            }
        }

        // PUT /api/v1/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Customer customer)
        {
            _logger.LogInformation("PUT /api/v1/customers/{Id} recibido.", id);

            if (customer == null || customer.Id != id)
                return BadRequest(new ActionMessageDto { Message = "El cliente es nulo o el ID no coincide." });

            try
            {
                bool updated = await _customerService.UpdateCustomerAsync(customer);
                if (!updated)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el cliente con ID {id} para actualizar." });

                var response = new CustomerResponseDto
                {
                    Id = customer.Id,
                    IdentificationNumber = customer.IdentificationNumber,
                    FirstName = customer.FirstName,
                    MiddleName = customer.MiddleName,
                    LastName1 = customer.LastName1,
                    LastName2 = customer.LastName2,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    Message = "El cliente fue actualizado de manera exitosa."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el cliente con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al actualizar el cliente." });
            }
        }

        // DELETE /api/v1/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /api/v1/customers/{Id} recibido.", id);

            try
            {
                bool deleted = await _customerService.DeleteCustomerAsync(id);
                if (!deleted)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró el cliente con ID {id} para eliminar." });

                return Ok(new ActionMessageDto { Message = "El cliente fue eliminado de manera exitosa." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el cliente con ID {Id}.", id);
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al eliminar el cliente." });
            }
        }
    }
}
