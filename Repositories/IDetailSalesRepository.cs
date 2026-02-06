using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IDetailSalesRepository
    {
        Task<IEnumerable<DetailSales>> GetAllAsync();
        Task<DetailSales?> GetByIdAsync(int saleId);
        Task<int> CreateAsync(DetailSales detailSales);
        Task<bool> UpdateAsync(DetailSales detailSales);
        Task<bool> DeleteAsync(int saleId);
    }
}