using SupermarketAPI.Models;

namespace SupermarketAPI.Services
{
    public interface IBranchService
    {
        Task<IEnumerable<Branch>> GetAllBranchesAsync();
        Task<Branch?> GetBranchByIdAsync(int id);
        Task<int> CreateBranchAsync(Branch branch);
        Task<bool> UpdateBranchAsync(Branch branch);
        Task<bool> DeleteBranchAsync(int id);
    }
}
