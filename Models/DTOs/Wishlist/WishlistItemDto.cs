namespace GameStore.Models.DTOs.Wishlist
{
    public class WishlistItemDto
    {
        public int WishlistItemId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
