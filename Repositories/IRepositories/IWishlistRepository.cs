using GameStore.Models.Entities;

namespace GameStore.Repositories.IRepositories
{
    public interface IWishlistRepository
    {
        Task<Wishlist> GetWishlistByUserIdAsync(int userId);
        Task<Wishlist> CreateWishlistAsync(Wishlist wishlist);
        Task AddWishlistItemAsync(WishlistItem item);
        Task RemoveWishlistItemAsync(int wishlistItemId);
        Task<bool> WishlistItemExistsAsync(int wishlistId, int gameId);
    }
}
