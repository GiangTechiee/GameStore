using GameStore.Data;
using GameStore.Models.Entities;
using GameStore.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories.MyRepositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameStoreContext _context;

        public GameRepository(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games
                .Include(g => g.GameCategories)
                    .ThenInclude(gc => gc.Category)
                .Include(g => g.GameImages)
                .ToListAsync();
        }

        public async Task<Game> GetByIdAsync(int id)
        {
            return await _context.Games
                .Include(g => g.GameCategories)
                    .ThenInclude(gc => gc.Category)
                .Include(g => g.GameImages)
                .FirstOrDefaultAsync(g => g.GameId == id);
        }

        public async Task AddAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Game game)
        {
            // Lấy danh sách GameCategories hiện có từ cơ sở dữ liệu
            var existingCategories = await _context.GameCategories
                .Where(gc => gc.GameId == game.GameId)
                .ToListAsync();

            // Danh sách CategoryIds mới từ game
            var newCategoryIds = game.GameCategories.Select(gc => gc.CategoryId).ToList();

            // Xóa các GameCategories không còn trong danh sách mới
            var categoriesToRemove = existingCategories
                .Where(ec => !newCategoryIds.Contains(ec.CategoryId))
                .ToList();
            _context.GameCategories.RemoveRange(categoriesToRemove);

            // Thêm các GameCategories mới (chưa tồn tại)
            var existingCategoryIds = existingCategories.Select(ec => ec.CategoryId).ToList();
            var categoriesToAdd = game.GameCategories
                .Where(gc => !existingCategoryIds.Contains(gc.CategoryId))
                .ToList();
            _context.GameCategories.AddRange(categoriesToAdd);

            // Cập nhật entity Game
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteGameImagesAsync(int gameId)
        {
            var existingImages = _context.GameImages.Where(img => img.GameId == gameId);
            _context.GameImages.RemoveRange(existingImages);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Game>> GetByPlatformAsync(string platform)
        {
            return await _context.Games
                .Where(g => g.Platform.Contains(platform))
                .Include(g => g.GameCategories)
                    .ThenInclude(gc => gc.Category)
                .Include(g => g.GameImages)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByReleaseDateDescAsync()
        {
            return await _context.Games
                .Include(g => g.GameCategories)
                    .ThenInclude(gc => gc.Category)
                .Include(g => g.GameImages)
                .OrderByDescending(g => g.ReleaseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByViewCountDescAsync()
        {
            return await _context.Games
                .Include(g => g.GameCategories)
                    .ThenInclude(gc => gc.Category)
                .Include(g => g.GameImages)
                .OrderByDescending(g => g.ViewCount)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByGOTYAsync(bool isGOTY)
        {
            return await _context.Games
                .Where(g => g.IsGOTY == isGOTY)
                .Include(g => g.GameCategories)
                    .ThenInclude(gc => gc.Category)
                .Include(g => g.GameImages)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Games
                .Where(g => g.GameCategories.Any(gc => gc.CategoryId == categoryId))
                .Include(g => g.GameCategories)
                    .ThenInclude(gc => gc.Category)
                .Include(g => g.GameImages)
                .ToListAsync();
        }
    }
}
