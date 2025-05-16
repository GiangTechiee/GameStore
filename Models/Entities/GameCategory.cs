using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.Entities
{
    public class GameCategory
    {
        [Key]
        [Column(Order = 1)]
        public int GameId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int CategoryId { get; set; }

        [ForeignKey("GameId")]
        public Game Game { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
