using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DTOs.Cart
{
    public class UpdateCartDetailDto
    {
        [Required]
        public int CartDetailId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
        public decimal UnitPrice { get; set; }
    }
}
