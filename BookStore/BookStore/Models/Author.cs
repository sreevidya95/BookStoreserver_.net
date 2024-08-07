using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    /// <summary>
    /// Author's Details
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Author Id
        /// </summary>
        public int author_id { get; set; }
        /// <summary>
        /// Name of author
        /// </summary>
        public string? name { get; set; }
        /// <summary>
        /// author's biography
        /// </summary>
        public string? biography { get; set; }
        /// <summary>
        /// Image of Author
        /// </summary>
        public byte[]? author_image { get; set; } = null;
        /// <summary>
        /// created At
        /// </summary>
       
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        /// <summary>
        /// Updated At
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
