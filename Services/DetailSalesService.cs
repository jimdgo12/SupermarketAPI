using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services
{
    public class DetailSalesService : IDetailSalesService
    {
        private readonly IDetailSalesRepository _repository;

        public DetailSalesService(IDetailSalesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DetailSales>> GetAllDetailSalesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<DetailSales?> GetDetailSalesByIdAsync(int saleId)
        {
            return await _repository.GetByIdAsync(saleId);
        }

        public async Task<int> CreateDetailSalesAsync(DetailSales detailSales)
        {
            return await _repository.CreateAsync(detailSales);
        }

        public async Task<bool> UpdateDetailSalesAsync(DetailSales detailSales)
        {
            return await _repository.UpdateAsync(detailSales);
        }

        public async Task<bool> DeleteDetailSalesAsync(int saleId)
        {
            return await _repository.DeleteAsync(saleId);
        }
    }
}