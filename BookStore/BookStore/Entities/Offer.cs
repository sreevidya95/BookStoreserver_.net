using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Entities
{
    public class Offer
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int offer_id { get; set; }
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string? name { get; set; }
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal discount { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime startDate { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime endDate { get; set; }
    }
}
