using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DTOs.Game
{
    public class CreateGameDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
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

        public bool IsGOTY { get; set; } = false;

        [Required]
        public List<int> CategoryIds { get; set; } = new List<int>();

        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}
