using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repository;

        public SaleService(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateSaleAsync(Sale sale)
        {
            // Aquí podrías agregar lógica de negocio adicional 
            // (por ejemplo, validar que el TotalAmount coincida con los subtotales)
            return await _repository.CreateAsync(sale);
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}