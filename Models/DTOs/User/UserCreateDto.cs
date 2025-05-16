using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DTOs.User
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(256)]
        public string Password { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
