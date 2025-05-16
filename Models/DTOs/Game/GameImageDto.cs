using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DTOs.Game
{
    public class GameImageDto
    {
        public int ImageId { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
