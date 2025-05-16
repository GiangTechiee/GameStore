using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.Entities
{
    public class WishlistItem
    {
        [Key]
        public int WishlistItemId { get; set; }

        [Required]
        public int WishlistId { get; set; }

        [ForeignKey("WishlistId")]
        public Wishlist Wishlist { get; set; }

        [Required]
        public int GameId { get; set; }

        [ForeignKey("GameId")]
        public Game Game { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}
