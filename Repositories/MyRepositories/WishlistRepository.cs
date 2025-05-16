using GameStore.Data;
using GameStore.Models.Entities;
using GameStore.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories.MyRepositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly GameStoreContext _context;

        public WishlistRepository(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<Wishlist> GetWishlistByUserIdAsync(int userId)
        {
            return await _context.Wishlists
                .Include(w => w.WishlistItems)
                .ThenInclude(wi => wi.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<Wishlist> CreateWishlistAsync(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
            await _context.SaveChangesAsync();
            return wishlist;
        }

        public async Task AddWishlistItemAsync(WishlistItem item)
        {
            await _context.WishlistItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveWishlistItemAsync(int wishlistItemId)
        {
            var item = await _context.WishlistItems
                .FirstOrDefaultAsync(wi => wi.WishlistItemId == wishlistItemId);

            if (item != null)
            {
                _context.WishlistItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> WishlistItemExistsAsync(int wishlistId, int gameId)
        {
            return await _context.WishlistItems
                .AnyAsync(wi => wi.WishlistId == wishlistId && wi.GameId == gameId);
        }
    }
}
