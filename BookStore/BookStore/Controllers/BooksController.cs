using AutoMapper;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;

namespace BookStore.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookStoreRepository bookStore;
        private readonly IMapper mapper;
        private readonly ILogger<BooksController> log;

        public BooksController(IBookStoreRepository bookStore,IMapper mapper,ILogger<BooksController> log) {
            this.bookStore = bookStore;
            this.mapper = mapper;
            this.log = log;
        }
        //getting all books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Books?>>> getBooksAsync()
        {
            try
            {
                var books = await bookStore.GetBooksasync();
                if (books == null)
                {
                    return NotFound("couldnt find any books");
                }
                else
                {


                    var bookEntity = mapper.Map<IEnumerable<Models.Books>>(books);
                    for (int i = 0; i < bookEntity.Count(); i++)
                    {
                        if(bookEntity.ElementAt(i).book_image!=null)
                        bookEntity.ElementAt(i).book_image = bookStore.Image(Convert.ToBase64String(bookEntity.ElementAt(i).book_image));


                    }
                    return Ok(bookEntity);
                }
            }
            catch (Exception ex) { 
                 return BadRequest(ex.Message);
            }
        }
        //getting specific book
        [HttpGet("{id}",Name ="getBook")]
        public async Task<ActionResult<Models.Books?>> GetBooksById(int id)
        {
            try
            {
                var book = await bookStore.GetBooksByIdAsync(id);
                if (book == null)
                {
                    return NotFound(JsonConvert.SerializeObject(new
                    {
                        status=404,
                        msg= "Couldnt find the book"
                    }));
                }
                else
                {

                    var bookEntity = mapper.Map<Models.Books>(book);
                    if (book.offerOfferId != null)
                    {
                        bookEntity.offerOfferId = book.offerOfferId;
                    }
                    if (book.book_image != null) {
                        bookEntity.book_image = bookStore.Image(Convert.ToBase64String(book.book_image));
                    }
                    return bookEntity;
                }
            } catch (Exception ex) { 
            
                  return BadRequest(ex.Message );
            }
        }
        [HttpGet("sort/{sort}")]
        public async Task<ActionResult<IEnumerable<Models.Books>>> GetBooksBySort(string sort)
        {
            try
            {
                var book = await bookStore.GetBooksBySortAsync(sort);
                if (book == null)
                {
                    return NotFound(JsonConvert.SerializeObject(new
                    {
                        status = 404,
                        msg = "Couldnt find the book"
                    }));
                }
                else
                {

                    return Ok(mapper.Map<IEnumerable<Models.Books>>(book));
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        //deleting single book
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await bookStore.DeleteBooksAsync(id);
                bool changes = await bookStore.SyncDb();
                if (changes == true)
                {
                    log.LogInformation($"A book with id '{id}' is delete");
                    return NoContent();
                }
                else
                {
                    return NotFound("Couldnt find the book");
                }
            }
            catch (Exception ex) {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Models.Books>> CreateBook(UpdateBook book)
        {
            try
            {
                var bookEntity = new Entities.Books
                {
                    title=book.title,
                    price = book.price,
                    publication_date = book.publication_date,
                    AuthorAuthorId = book.AuthorAuthorId,
                    GenreGenreId = book.GenreGenreId,
                    offerOfferId = book.offerOfferId

                };
                if (book.book_image != null)
                {
                    var file = book.book_image;
                    //read the file
                    using (var memoryStream = new MemoryStream())
                    {
                       file.CopyTo(memoryStream);
                       var fileBytes= memoryStream.ToArray();
                        bookEntity.book_image = fileBytes;
                    }
                }

                await bookStore.CreateBook(bookEntity);
                bool added = await bookStore.SyncDb();
                if (added == true)
                {

                    var bookModel = mapper.Map<Models.Books>(bookEntity);
                    log.LogInformation($"The Book '{bookModel.title}' is added");
                    return CreatedAtRoute("getBook", new
                    {
                        id = bookModel.book_id
                    }, bookModel);

                }
                else
                {
                    return Ok("Book already Exists");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Models.Books?>> UpdateBook(int id, UpdateBook book)
        {
            try
            {
                var bookCurrent = await bookStore.GetOnlyBooksAsync(id);
                if (bookCurrent == null)
                {
                    return NotFound("Invalid Book");
                }
                else
                {
                    if (book.book_image != null)
                    {
                        var file = book.book_image;
                        //read the file
                        using (var memoryStream = new MemoryStream())
                        {
                            file.CopyTo(memoryStream);
                            var fileBytes = memoryStream.ToArray();
                            bookCurrent.book_image = fileBytes;
                        }
                    }
                    bookCurrent.title = book.title;
                    bookCurrent.price = book.price;
                    bookCurrent.publication_date = book.publication_date;
                    book.offerOfferId= book.offerOfferId;
                    book.AuthorAuthorId = book.AuthorAuthorId;
                    book.GenreGenreId = book.GenreGenreId;
                    bool updated = await bookStore.SyncDb();
                    var model = mapper.Map<Models.Books>(bookCurrent);
                    if (updated == true)
                    {
                       
                        log.LogInformation($"The Book with id '{bookCurrent.book_id}' is updated");
                       
                       

                    }
                    return Ok(model);

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
