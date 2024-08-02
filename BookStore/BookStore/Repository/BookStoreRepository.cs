using BookStore.DBContext;
using BookStore.Entities;
using BookStore.Migrations;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class BookStoreRepository : IBookStoreRepository
    {
        private readonly BookStoreDbContext bookStoreDb;

        public BookStoreRepository(BookStoreDbContext bookStoreDb) {
            this.bookStoreDb = bookStoreDb;
        }
        public  Task<Books> CreateBooksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteBooksAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Books>> GetBooksasync()
        {
            return await (from book in bookStoreDb.Books
                          join auth in bookStoreDb.Authors
                          on book.AuthorAuthorId equals auth.author_id
                          join g in bookStoreDb.Genres
                          on book.GenreGenreId equals g.genre_id
                          orderby book.UpdatedAt descending
                          select new Books()
                          {
                              book_id = book.book_id,
                              title = book.title,
                              price = book.price,
                              publication_date= book.publication_date,
                              AuthorAuthorId=book.AuthorAuthorId,
                              Author = auth,
                              GenreGenreId=book.GenreGenreId,
                              book_image=book.book_image,
                              Genre = g,
                              CreatedAt = book.CreatedAt,
                              UpdatedAt = book.UpdatedAt,
                              offerOfferId = book.offerOfferId
                          }).ToListAsync();
        }

        public async Task<Books?> GetBooksByIdAsync(int id)
        {
            return await (from book in bookStoreDb.Books
                          join auth in bookStoreDb.Authors
                          on book.AuthorAuthorId equals auth.author_id
                          join g in bookStoreDb.Genres
                          on book.GenreGenreId equals g.genre_id
                          orderby book.UpdatedAt descending
                          where (book.book_id == id)
                          select new Books()
                          {
                              book_id = book.book_id,
                              title = book.title,
                              price = book.price,
                              publication_date = book.publication_date,
                              AuthorAuthorId = book.AuthorAuthorId,
                              Author = auth,
                              GenreGenreId = book.GenreGenreId,
                              book_image = book.book_image,
                              Genre = g,
                              CreatedAt = book.CreatedAt,
                              UpdatedAt = book.UpdatedAt,
                              offerOfferId = book.offerOfferId
                          }).FirstOrDefaultAsync();
        }

        public async Task<bool> SyncDb()
        {
            return await bookStoreDb.SaveChangesAsync() > 0;
        }
    }
}
