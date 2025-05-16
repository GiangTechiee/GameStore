using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DTOs.Cart
{
    public class AddToCartDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int GameId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
        public decimal UnitPrice { get; set; }
    }
}
