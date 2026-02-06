using SupermarketAPI.Models;

namespace SupermarketAPI.Services
{
    public interface IDetailSalesService
    {
        Task<IEnumerable<DetailSales>> GetAllDetailSalesAsync();
        Task<DetailSales?> GetDetailSalesByIdAsync(int saleId);
        Task<int> CreateDetailSalesAsync(DetailSales detailSales);
        Task<bool> UpdateDetailSalesAsync(DetailSales detailSales);
        Task<bool> DeleteDetailSalesAsync(int saleId);
    }
}
