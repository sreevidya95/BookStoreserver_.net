using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Entities
{
    public class Books
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int book_id { get; set; }
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string? title { get; set; }
        [Required]
        [Column(TypeName ="decimal(5,2)")]
        public decimal price { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime publication_date { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? book_image { get; set; } = null;
        [ForeignKey("AuthorAuthorId")]
        [Required]
        public Author? Author { get; set; }
        public int AuthorAuthorId { get; set; }
        [Required]
        [ForeignKey("GenreGenreId")]
        public Genre? Genre { get; set; }
        public int GenreGenreId { get; set; }
        [ForeignKey("offerOfferId")]
        public Offer? Offer { get; set; }
        public int? offerOfferId { get; set; } = null;
        [Required]
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


    }
}
