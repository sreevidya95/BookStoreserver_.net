using BookStore.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    /// <summary>
    /// Book Details
    /// </summary>
    public class Books
    {
        /// <summary>
        /// Book Id
        /// </summary>
        
        public int book_id { get; set; }
        /// <summary>
        /// Book Title
        /// </summary>
        public string? title { get; set; }
        /// <summary>
        /// Book Price
        /// </summary>
         public decimal price { get; set; }
        /// <summary>
        /// Book's Publication Date
        /// </summary>
        public DateTime publication_date { get; set; }
        /// <summary>
        /// Book's Image
        /// </summary>
        public byte[]? book_image { get; set; } = null;
        /// <summary>
        /// Author Details
        /// </summary>
        public Author? Author { get; set; }
        /// <summary>
        /// Author Id
        /// </summary>
        public int AuthorAuthorId { get; set; }
        /// <summary>
        /// Genre Details
        /// </summary>
        public Genre? Genre { get; set; }
        /// <summary>
        /// Genre Id
        /// </summary>
        public int GenreGenreId { get; set; }
        /// <summary>
        /// OfferId
        /// </summary>
         public int? offerOfferId { get; set; } = null;
         /// <summary>
         /// Created At
         /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
         /// <summary>
         /// Updated At
         /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
