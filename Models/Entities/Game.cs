using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models.Entities
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Platform { get; set; }

        [Required]
        public string Developers { get; set; }

        [Required]
        public string Publishers { get; set; }

        public string Website { get; set; }

        public int ViewCount { get; set; } = 0;

        public bool IsGOTY { get; set; } = false;

        public ICollection<GameCategory> GameCategories { get; set; } = new List<GameCategory>();

        public ICollection<GameImage> GameImages { get; set; } = new List<GameImage>();
    }
}
