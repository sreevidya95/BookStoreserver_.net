namespace BookStore.Models
{
    public class UpdateAuthor
    {
        
            public string? name { get; set; }

            public string? biography { get; set; }

            public IFormFile? author_image { get; set; } = null;

            public DateTime UpdatedAt { get; set; } = DateTime.Now;
       
    }

}

