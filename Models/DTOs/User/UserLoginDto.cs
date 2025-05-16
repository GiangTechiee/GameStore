using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DTOs.User
{
    public class UserLoginDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(256)]
        public string Password { get; set; }
    }
}
