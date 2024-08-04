using BCrypt.Net;
using BookStore.DBContext;
using BookStore.Entities;
using BookStore.Migrations;
using BookStore.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
namespace BookStore.Repository
{
    public class BookStoreRepository : IBookStoreRepository
    {
        private readonly BookStoreDbContext bookStoreDb;

        public BookStoreRepository(BookStoreDbContext bookStoreDb) {
            this.bookStoreDb = bookStoreDb;
        }

        public async Task DeleteBooksAsync(int id)
        {
            var books = await GetBooksByIdAsync(id);
            if (books != null)
            {
                bookStoreDb.Remove(books);
            }
        }

        public async Task<IEnumerable<Entities.Books>> GetBooksasync()
        {
            return await (from book in bookStoreDb.Books
                          join auth in bookStoreDb.Authors
                          on book.AuthorAuthorId equals auth.author_id
                          join g in bookStoreDb.Genres
                          on book.GenreGenreId equals g.genre_id
                          orderby book.UpdatedAt descending
                          select new Entities.Books()
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

        public async Task<Entities.Books?> GetBooksByIdAsync(int id)
        {
            return await (from book in bookStoreDb.Books
                          join auth in bookStoreDb.Authors
                          on book.AuthorAuthorId equals auth.author_id
                          join g in bookStoreDb.Genres
                          on book.GenreGenreId equals g.genre_id
                          orderby book.UpdatedAt descending
                          where (book.book_id == id)
                          select new Entities.Books()
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
        public async Task<Entities.Books?> GetOnlyBooksAsync(int id)
        {
            return await bookStoreDb.Books.Where(x => x.book_id == id).FirstOrDefaultAsync();
        }
        public async Task<bool> SyncDb()
        {
            return await bookStoreDb.SaveChangesAsync() > 0;
        }

       
        public async Task CreateGenre(Entities.Genre genre)
        {
           var exists = await bookStoreDb.Genres.
                Where(x => x.genre_name == genre.genre_name).FirstOrDefaultAsync();
            if (exists == null)
            {
                 bookStoreDb.Genres.Add(genre);
            }

        }
        public async Task<IEnumerable<Entities.Genre>> GetGenresAsync()
        {

            return await bookStoreDb.Genres.ToListAsync();

        }

        public async Task<Entities.Genre?> GetGenresByIdAsync(int id)
        {
            return await bookStoreDb.Genres.Where(x => x.genre_id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Entities.Author>> GetAuthorAsync()
        {
            return await bookStoreDb.Authors.OrderByDescending(x=>x.UpdatedAt).ToListAsync();

        }
        public async Task<Entities.Author?> GetAuthorByIdAsync(int id)
        {
            return await bookStoreDb.Authors.Where(x => x.author_id == id).FirstOrDefaultAsync();
        }
        public async  Task DeleteAuthorAsync(int id)
        {
            var author = await GetAuthorByIdAsync(id);
            if (author != null)
            {
                var books = await bookStoreDb.Books.Where(x => x.AuthorAuthorId == id).FirstOrDefaultAsync();
                if (books == null)
                {
                    bookStoreDb.Remove(author);
                }
            }
        }
        
        public async Task CreateAdminAsync(Entities.Admin admin)
        {
            var adminConfirmation = await LoginAdmin(admin.email);
            if (adminConfirmation == null)
            {
                bookStoreDb.Admins.Add(admin);
            }

        }
        public async Task<Entities.Admin?> LoginAdmin(string email)
        {
            return await bookStoreDb.Admins.Where(x => x.email == email)
                   .FirstOrDefaultAsync();
        }
        public async Task CreateAuthor(Entities.Author author)
        {

            var newAuthor = await bookStoreDb.Authors.Where(x =>
            x.name == author.name && x.biography == author.biography).FirstOrDefaultAsync();

            if (newAuthor == null)
            {
                bookStoreDb.Authors.Add(author);
            }
        }
        public async Task CreateBook(Entities.Books book)
        {
            var books = await bookStoreDb.Books.Where(x=>x.title == book.title 
            && x.AuthorAuthorId == book.AuthorAuthorId && x.
            GenreGenreId == book.GenreGenreId).FirstOrDefaultAsync();
            if (books == null)
            {
                Console.WriteLine("Adding");
                bookStoreDb.Books.Add(book);
            }
        }
        /*public IFormFile GetFile(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var file = new FormFile(stream, 0, data.Length,"author_image","author_image")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/html",
                };

                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = file.FileName
                };
                file.ContentDisposition = cd.ToString();
                return file;
            }*/
        public async Task<IEnumerable<Entities.Enquiry>> GetEnquiry()
        {
            return await bookStoreDb.Enquiries.ToListAsync();
        }
        public async Task CreateEnquiry(Entities.Enquiry enquiry)
        {
            var Enq = await bookStoreDb.Enquiries.Where(x => x.user_email == enquiry.user_email
            && x.message == enquiry.message).FirstOrDefaultAsync();
            if (Enq == null)
            {
                bookStoreDb.Add(enquiry);
            }
        }
        public async Task DeleteEnquiry(int id)
        {
            var enquiry = await GetEnquiryById(id);
            if (enquiry != null)
            {
                bookStoreDb.Remove(enquiry);
            }
        }
        public async Task<Entities.Enquiry?> GetEnquiryById(int id)
        {
            return await bookStoreDb.Enquiries.Where(x => x.enq_id == id).FirstOrDefaultAsync();
        }
    }
            
}

        


