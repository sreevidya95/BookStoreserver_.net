using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Entities
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int author_id { get; set; }
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string? name { get; set; }
        [Required]
        [MaxLength(200)]
        [Column(TypeName = "varchar")]
        public string? biography { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? author_image { get; set; } = null;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;




    }
}
