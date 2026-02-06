using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface ISaleRepository
    {
        // Método para crear la venta y devolver el ID generado
        Task<int> CreateAsync(Sale sale);

        // Método para obtener todas las ventas (historial)
        Task<IEnumerable<Sale>> GetAllAsync();

        // Método para obtener una venta específica por su ID
        Task<Sale?> GetByIdAsync(int id);
    }
}