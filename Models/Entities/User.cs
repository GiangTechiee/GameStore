using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace GameStore.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public Cart Cart { get; set; }

        public Wishlist Wishlist { get; set; }

        

    }
}
