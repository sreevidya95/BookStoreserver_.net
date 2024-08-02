using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Genre
    {
        public int genre_id { get; set; }
        public string? genre_name { get; set; }
    }
}
