using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<GameCategory> GameCategories { get; set; } = new List<GameCategory>();
    }
}
