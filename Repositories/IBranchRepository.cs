using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IBranchRepository
    {
        Task<IEnumerable<Branch>> GetAllAsync();
        Task<Branch?> GetByIdAsync(int id);
        Task<int> CreateAsync(Branch branch);
        Task<bool> UpdateAsync(Branch branch);
        Task<bool> DeleteAsync(int id);
    }
}
