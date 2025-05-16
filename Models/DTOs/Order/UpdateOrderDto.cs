
using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DTOs.Order
{
    public class UpdateOrderDto
    {
        [Required]
        [StringLength(20)]
        public string Status { get; set; }
    }
}
