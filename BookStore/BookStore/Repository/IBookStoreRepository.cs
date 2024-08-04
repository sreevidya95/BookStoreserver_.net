using BookStore.Entities;

namespace BookStore.Repository
{
    public interface IBookStoreRepository
    {
        //crud for books table
        public Task<IEnumerable<Books>> GetBooksasync();
        public Task<Books?> GetBooksByIdAsync(int id);
       
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
        //public IFormFile GetFile(byte[] data);

    }
}
