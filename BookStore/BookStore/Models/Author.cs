using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Author
    {
        
        public int author_id { get; set; }
        
        public string? name { get; set; }
        
        public string? biography { get; set; }
        
        public byte[]? author_image { get; set; } = null;

       
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
