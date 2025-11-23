using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _repository;

        public BranchService(IBranchRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Branch?> GetBranchByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> CreateBranchAsync(Branch branch)
        {
            return await _repository.CreateAsync(branch);
        }

        public async Task<bool> UpdateBranchAsync(Branch branch)
        {
            return await _repository.UpdateAsync(branch);
        }

        public async Task<bool> DeleteBranchAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
