using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Entities
{
    public class Enquiry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int enq_id { get; set; }
        [Required]
        public string? message { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? user_email { get; set; }
        [Required]
        public byte isRead { get; set; } = 0;
    }
}
