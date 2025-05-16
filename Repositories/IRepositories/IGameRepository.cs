using GameStore.Models.Entities;

namespace GameStore.Repositories.IRepositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game> GetByIdAsync(int id);
        Task AddAsync(Game game);
        Task UpdateAsync(Game game);
        Task DeleteAsync(int id);
        Task DeleteGameImagesAsync(int gameId);

        Task<IEnumerable<Game>> GetByPlatformAsync(string platform);
        Task<IEnumerable<Game>> GetByReleaseDateDescAsync();
        Task<IEnumerable<Game>> GetByViewCountDescAsync();
        Task<IEnumerable<Game>> GetByGOTYAsync(bool isGOTY);
        Task<IEnumerable<Game>> GetByCategoryAsync(int categoryId);
    }
}
