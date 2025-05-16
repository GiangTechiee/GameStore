using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DTOs.Game
{
    public class GameDto
    {
        public int GameId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
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

        public int ViewCount { get; set; }

        public bool IsGOTY { get; set; }

        [Required]
        public List<int> CategoryIds { get; set; } = new List<int>();

        public List<string> CategoryNames { get; set; } = new List<string>();

        public List<GameImageDto> GameImages { get; set; } = new List<GameImageDto>();
    }
}
