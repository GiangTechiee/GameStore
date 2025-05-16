using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.Entities
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required, MaxLength(50)]
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
