using BookStore.Entities;

namespace BookStore.Repository
{
    public interface IBookStoreRepository
    {
        //crud for books table
        public Task<IEnumerable<Books>> GetBooksasync();
        public Task<Books?> GetBooksByIdAsync(int id);
        public Task<IEnumerable<Entities.Books>> GetBooksBySortAsync(string sort);

        //update will get update when we sync the model and entity 
        public Task DeleteBooksAsync(int id);
        public Task<IEnumerable<Genre>> GetGenresAsync();
        public Task<Genre?> GetGenresByIdAsync(int id);
        public Task CreateGenre(Entities.Genre genre);
        public Task<IEnumerable<Entities.Author>> GetAuthorAsync();
        public Task DeleteAuthorAsync(int id);
        public Task<Entities.Author?> GetAuthorByIdAsync(int id);

        public Task CreateAdminAsync(Entities.Admin admin);
       
        public Task<Admin?> LoginAdmin(string email);
       public Task CreateAuthor(Entities.Author author);
        public Task<bool> SyncDb();
        public Task CreateBook(Entities.Books book);
        public  Task CreateEnquiry(Entities.Enquiry enquiry);
        public Task<IEnumerable<Entities.Enquiry>> GetEnquiry();
        public Task<Entities.Enquiry?> GetEnquiryById(int id);
        public Task<Entities.Books?> GetOnlyBooksAsync(int id);
        public Task DeleteEnquiry(int id);
        public byte[] Image(string image);
        public Task<IEnumerable<Entities.Books?>> GetAuthorAuthorIdAsync(int id);
        //public IFormFile GetFile(byte[] data);
        //public Task CreateOffer(Entities.Offer offer);

    }
}
