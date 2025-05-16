namespace GameStore.Models.DTOs.Wishlist
{
    public class WishlistDto
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public List<WishlistItemDto> WishlistItems { get; set; } = new List<WishlistItemDto>();
    }
}
