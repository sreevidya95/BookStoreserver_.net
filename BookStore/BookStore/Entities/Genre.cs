using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Entities
{
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int genre_id { get; set; }
        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar")]
        public string? genre_name { get; set; }
    }
}
