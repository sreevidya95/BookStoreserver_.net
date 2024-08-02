using BookStore.Entities;

namespace BookStore.Repository
{
    public interface IBookStoreRepository
    {
        //crud for books table
        public Task<IEnumerable<Books>> GetBooksasync();
        public Task<Books?> GetBooksByIdAsync(int id);
        public Task<Books> CreateBooksAsync();
        //update will get update when we sync the model and entity 
        public Task DeleteBooksAsync(int id);
        public Task<bool> SyncDb();

    }
}
