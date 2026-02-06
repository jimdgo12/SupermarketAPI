using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using SupermarketAPI.Dtos;

namespace SupermarketAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DetailSalesController : ControllerBase
    {
        private readonly IDetailSalesService _detailSalesService;
        private readonly ILogger<DetailSalesController> _logger;

        public DetailSalesController(IDetailSalesService detailSalesService, ILogger<DetailSalesController> logger)
        {
            _detailSalesService = detailSalesService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var details = await _detailSalesService.GetAllDetailSalesAsync();
            var response = details.Select(d => new DetailSalesResponseDto
            {
                SaleId = d.SaleId,
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Subtotal = d.Subtotal,
                Message = "Detalle cargado correctamente."
            });
            return Ok(response);
        }

        [HttpGet("{saleId}")]
        public async Task<IActionResult> GetById(int saleId)
        {
            var detail = await _detailSalesService.GetDetailSalesByIdAsync(saleId);
            if (detail == null)
                return NotFound(new ActionMessageDto { Message = $"No se encontró el detalle con SaleId {saleId}." });

            var response = new DetailSalesResponseDto
            {
                SaleId = detail.SaleId,
                ProductId = detail.ProductId,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice,
                Subtotal = detail.Subtotal,
                Message = "Detalle encontrado."
            };
            return Ok(response);
        }
    }
}