using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using SupermarketAPI.Dtos;

namespace SupermarketAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly ILogger<SalesController> _logger;

        public SalesController(ISaleService saleService, ILogger<SalesController> logger)
        {
            _saleService = saleService ?? throw new ArgumentNullException(nameof(saleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // POST: api/v1/sales
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sale sale)
        {
            _logger.LogInformation("🚀 Recibida solicitud POST para nueva venta.");

            if (sale == null || sale.Details == null || !sale.Details.Any())
            {
                return BadRequest(new ActionMessageDto { Message = "La venta debe contener al menos un producto en el detalle." });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // El servicio se encarga de la transacción (Venta + Detalle + Inventario)
                int newId = await _saleService.CreateSaleAsync(sale);

                _logger.LogInformation("✅ Venta creada exitosamente con ID: {Id}", newId);

                var response = new SaleResponseDto
                {
                    Id = newId,
                    TotalAmount = sale.TotalAmount,
                    SaleDate = DateTime.Now,
                    Message = "La venta fue procesada y el inventario actualizado correctamente."
                };

                return CreatedAtAction(nameof(GetById), new { id = newId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error al procesar la venta.");
                return StatusCode(500, new ActionMessageDto
                {
                    Message = "Error interno al procesar la venta. Verifique que haya stock suficiente y que los IDs de Cliente/Empleado existan."
                });
            }
        }

        // GET: api/v1/sales
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("🔍 Consultando historial de ventas.");
            try
            {
                var sales = await _saleService.GetAllSalesAsync();
                if (!sales.Any())
                {
                    return Ok(new { Message = "No hay ventas registradas.", Data = sales });
                }
                return Ok(sales);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error al obtener las ventas.");
                return StatusCode(500, new ActionMessageDto { Message = "Error al obtener el historial de ventas." });
            }
        }

        // GET: api/v1/sales/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("🔍 Buscando venta con ID: {Id}", id);
            // Nota: Podrías implementar GetSaleByIdAsync en tu servicio si lo necesitas
            return Ok(new { Message = "Detalle de venta individual (En desarrollo)." });
        }
    }
}