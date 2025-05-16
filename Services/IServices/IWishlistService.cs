using GameStore.Models.DTOs.Wishlist;

namespace GameStore.Services.IServices
{
    public interface IWishlistService
    {
        Task<WishlistDto> GetWishlistByUserIdAsync(int userId);
        Task<WishlistItemDto> AddWishlistItemAsync(int userId, WishlistItemCreateDto createDto);
        Task<bool> RemoveWishlistItemAsync(int userId, int wishlistItemId);
    }
}
