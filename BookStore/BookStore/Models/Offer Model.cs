using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Offer_Model
    {
        public int offer_id { get; set; }
       
        public string? name { get; set; }
        
        public decimal discount { get; set; }
        
        public DateTime startDate { get; set; }
       
        public DateTime endDate { get; set; }
        public int[]? books { get; set; }
    }
}
