using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

        // 🔹 Obtener todos los roles
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _repository.GetAllAsync();
        }

        // 🔹 Obtener rol por ID
        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // 🔹 Crear rol
        public async Task<int> CreateRoleAsync(Role role)
        {
            return await _repository.CreateAsync(role);
        }

        // 🔹 Actualizar rol
        public async Task<bool> UpdateRoleAsync(Role role)
        {
            return await _repository.UpdateAsync(role);
        }

        // 🔹 Eliminar rol
        public async Task<bool> DeleteRoleAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
