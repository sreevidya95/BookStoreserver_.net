using BookStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DBContext
{
    public class BookStoreDbContext:DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) {
        
        }
        public DbSet<Books> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
    }
}
