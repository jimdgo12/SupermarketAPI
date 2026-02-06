using SupermarketAPI.Models;

namespace SupermarketAPI.Services
{
    public interface ISaleService
    {
        Task<int> CreateSaleAsync(Sale sale);
        Task<IEnumerable<Sale>> GetAllSalesAsync();
    }
}