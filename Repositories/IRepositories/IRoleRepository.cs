using GameStore.Models.Entities;

namespace GameStore.Repositories.IRepositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(int id);
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(int id);
        Task<Role> GetByNameAsync(string roleName);
    }
}
