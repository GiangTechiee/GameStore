using GameStore.Models.DTOs.Wishlist;
using GameStore.Models.Entities;
using GameStore.Repositories.IRepositories;
using GameStore.Services.IServices;

namespace GameStore.Services.MyServices
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;

        public WishlistService(IWishlistRepository wishlistRepository, IGameRepository gameRepository, IUserRepository userRepository)
        {
            _wishlistRepository = wishlistRepository;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
        }

        public async Task<WishlistDto> GetWishlistByUserIdAsync(int userId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            if (wishlist == null)
            {
                return null;
            }

            return new WishlistDto
            {
                WishlistId = wishlist.WishlistId,
                UserId = wishlist.UserId,
                WishlistItems = wishlist.WishlistItems.Select(wi => new WishlistItemDto
                {
                    WishlistItemId = wi.WishlistItemId,
                    GameId = wi.GameId,
                    GameName = wi.Game.Title, // Giả định Game có thuộc tính Title
                    AddedAt = wi.AddedAt
                }).ToList()
            };
        }

        public async Task<WishlistItemDto> AddWishlistItemAsync(int userId, WishlistItemCreateDto createDto)
        {
            // Kiểm tra người dùng tồn tại
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Kiểm tra game tồn tại
            var game = await _gameRepository.GetByIdAsync(createDto.GameId);
            if (game == null)
            {
                throw new InvalidOperationException("Game not found.");
            }

            // Lấy hoặc tạo wishlist
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            if (wishlist == null)
            {
                wishlist = new Wishlist { UserId = userId };
                await _wishlistRepository.CreateWishlistAsync(wishlist);
            }

            // Kiểm tra game đã có trong wishlist chưa
            if (await _wishlistRepository.WishlistItemExistsAsync(wishlist.WishlistId, createDto.GameId))
            {
                throw new InvalidOperationException("Game is already in the wishlist.");
            }

            // Thêm wishlist item
            var wishlistItem = new WishlistItem
            {
                WishlistId = wishlist.WishlistId,
                GameId = createDto.GameId,
                AddedAt = DateTime.Now
            };

            await _wishlistRepository.AddWishlistItemAsync(wishlistItem);

            return new WishlistItemDto
            {
                WishlistItemId = wishlistItem.WishlistItemId,
                GameId = wishlistItem.GameId,
                GameName = game.Title, // Giả định Game có thuộc tính Title
                AddedAt = wishlistItem.AddedAt
            };
        }

        public async Task<bool> RemoveWishlistItemAsync(int userId, int wishlistItemId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            if (wishlist == null || !wishlist.WishlistItems.Any(wi => wi.WishlistItemId == wishlistItemId))
            {
                return false;
            }

            await _wishlistRepository.RemoveWishlistItemAsync(wishlistItemId);
            return true;
        }
    }
}
