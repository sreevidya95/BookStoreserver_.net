namespace BookStore.Models
{
    public class UpdateBook
    {
        public string? title { get; set; }
        public decimal price { get; set; }
        public DateTime publication_date { get; set; }
        public byte[]? book_image { get; set; } = null;
        public int AuthorAuthorId { get; set; }
        public int GenreGenreId { get; set; }
        public int? offerOfferId { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
