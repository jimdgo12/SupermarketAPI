using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using SupermarketAPI.Dtos;

namespace SupermarketAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        // ✅ Crear nuevo inventario
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Inventory inventory)
        {
            _logger.LogInformation("POST /api/v1/inventory recibido.");

            if (inventory == null)
                return BadRequest(new ActionMessageDto { Message = "El cuerpo de la solicitud no contiene un inventario válido." });

            try
            {
                bool created = await _inventoryService.CreateInventoryAsync(inventory);
                if (!created)
                    return Conflict(new ActionMessageDto { Message = "Ya existe un inventario con esa sucursal y producto." });

                var response = new InventoryResponseDto
                {
                    BranchId = inventory.BranchId,
                    ProductId = inventory.ProductId,
                    StockQuantity = inventory.StockQuantity,
                    Message = "Inventario creado exitosamente."
                };

                return CreatedAtAction(nameof(GetById), new { branchId = inventory.BranchId, productId = inventory.ProductId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el inventario.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al crear el inventario." });
            }
        }

        // ✅ Obtener todo el inventario
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/v1/inventory recibido.");

            try
            {
                var inventoryList = await _inventoryService.GetAllInventoryAsync();

                if (inventoryList == null || !inventoryList.Any())
                    return Ok(new { message = "No hay registros de inventario.", data = new List<Inventory>() });

                var response = inventoryList.Select(i => new InventoryResponseDto
                {
                    BranchId = i.BranchId,
                    ProductId = i.ProductId,
                    StockQuantity = i.StockQuantity,
                    Message = "Inventario cargado correctamente."
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el inventario.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener el inventario." });
            }
        }

        // ✅ Obtener inventario por BranchId y ProductId
        [HttpGet("{branchId}/{productId}")]
        public async Task<IActionResult> GetById(int branchId, int productId)
        {
            _logger.LogInformation("GET /api/v1/inventory/{BranchId}/{ProductId} recibido.", branchId, productId);

            try
            {
                var inventory = await _inventoryService.GetInventoryByIdAsync(branchId, productId);
                if (inventory == null)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró inventario con BranchId {branchId} y ProductId {productId}." });

                var response = new InventoryResponseDto
                {
                    BranchId = inventory.BranchId,
                    ProductId = inventory.ProductId,
                    StockQuantity = inventory.StockQuantity,
                    Message = "Inventario encontrado."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener inventario por clave compuesta.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener el inventario." });
            }
        }

        // ✅ Obtener inventario por ProductId (todas las sucursales)
        [HttpGet("by-product/{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            _logger.LogInformation("GET /api/v1/inventory/by-product/{ProductId} recibido.", productId);

            try
            {
                var inventoryList = await _inventoryService.GetInventoryByProductAsync(productId);

                if (inventoryList == null || !inventoryList.Any())
                    return NotFound(new ActionMessageDto { Message = $"No se encontró inventario para el producto con ID {productId}." });

                var response = inventoryList.Select(i => new InventoryResponseDto
                {
                    BranchId = i.BranchId,
                    ProductId = i.ProductId,
                    StockQuantity = i.StockQuantity,
                    Message = "Inventario por producto cargado correctamente."
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener inventario por producto.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al obtener el inventario por producto." });
            }
        }

        // ✅ Actualizar inventario completo
        [HttpPut("{branchId}/{productId}")]
        public async Task<IActionResult> Put(int branchId, int productId, [FromBody] Inventory inventory)
        {
            _logger.LogInformation("PUT /api/v1/inventory/{BranchId}/{ProductId} recibido.", branchId, productId);

            if (inventory == null || inventory.BranchId != branchId || inventory.ProductId != productId)
                return BadRequest(new ActionMessageDto { Message = "El inventario es nulo o los identificadores no coinciden." });

            try
            {
                bool updated = await _inventoryService.UpdateInventoryAsync(inventory);
                if (!updated)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró inventario con BranchId {branchId} y ProductId {productId} para actualizar." });

                var response = new InventoryResponseDto
                {
                    BranchId = inventory.BranchId,
                    ProductId = inventory.ProductId,
                    StockQuantity = inventory.StockQuantity,
                    Message = "Inventario actualizado exitosamente."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el inventario.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al actualizar el inventario." });
            }
        }

        // ✅ Sumar stock a inventario existente
        [HttpPut("add-stock")]
        public async Task<IActionResult> AddStock([FromBody] InventoryAddStockDto dto)
        {
            _logger.LogInformation("PUT /api/v1/inventory/add-stock recibido.");

            if (dto == null || dto.QuantityToAdd <= 0)
                return BadRequest(new ActionMessageDto { Message = "La cantidad debe ser mayor a cero." });

            try
            {
                bool updated = await _inventoryService.AddStockAsync(dto.BranchId, dto.ProductId, dto.QuantityToAdd);
                if (!updated)
                    return NotFound(new ActionMessageDto { Message = "No se encontró el inventario para sumar stock." });

                return Ok(new ActionMessageDto { Message = "✅ Stock actualizado correctamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sumar stock.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al actualizar el stock." });
            }
        }

        // ✅ Eliminar inventario
        [HttpDelete("{branchId}/{productId}")]
        public async Task<IActionResult> Delete(int branchId, int productId)
        {
            _logger.LogInformation("DELETE /api/v1/inventory/{BranchId}/{ProductId} recibido.", branchId, productId);

            try
            {
                bool deleted = await _inventoryService.DeleteInventoryAsync(branchId, productId);
                if (!deleted)
                    return NotFound(new ActionMessageDto { Message = $"No se encontró inventario con BranchId {branchId} y ProductId {productId} para eliminar." });

                return Ok(new ActionMessageDto { Message = "Inventario eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el inventario.");
                return StatusCode(500, new ActionMessageDto { Message = "Error interno del servidor al eliminar el inventario." });
            }
        }
    }
}
