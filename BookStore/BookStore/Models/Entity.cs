using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Entity
    {
        public int enq_id { get; set; }
        
        public string? message { get; set; }
       
        public string? user_email { get; set; }
        
        public byte isRead { get; set; } = 0;
    }
}
