using AutoMapper;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;

namespace BookStore.Controllers
{
    /// <summary>
    /// Controller for books
    /// </summary>
    [Route("[Controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookStoreRepository bookStore;
        private readonly IMapper mapper;
        private readonly ILogger<BooksController> log;
        /// <summary>
        /// Books Controller Constructor
        /// </summary>
        /// <param name="bookStore">IbbokstoreRepo</param>
        /// <param name="mapper">automapper</param>
        /// <param name="log">serilog</param>
        public BooksController(IBookStoreRepository bookStore,IMapper mapper,ILogger<BooksController> log) {
            this.bookStore = bookStore;
            this.mapper = mapper;
            this.log = log;
        }
        /// <summary>
        /// Get All the Books from Database
        /// </summary>
        /// <returns>Books</returns>
        //getting all books
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <summary>
        /// Get Specific book from BookStore Database
        /// </summary>
        /// <param name="id"> book id</param>
        /// <returns>Specific Book</returns>
        //getting specific book
        [HttpGet("{id}",Name ="getBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <summary>
        /// Get The Books Belongs to Particular Author
        /// </summary>
        /// <param name="id"> Author Id(Foreignkey in book table)</param>
        /// <returns>Books Related to Author</returns>
        [HttpGet("Author/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Models.Books>>> GetBookByAuthor(int id)
        {
            try
            {
                var bookEntity = await bookStore.GetAuthorAuthorIdAsync(id);
                if (bookEntity == null)
                {
                    return NotFound("Couldnt find the book of the author");

                }
                else
                {
                    var model = mapper.Map<IEnumerable<Models.Books>>(bookEntity);
                    for (int i = 0; i < model.Count(); i++)
                    {
                        if (model.ElementAt(i).book_image != null)
                            model.ElementAt(i).book_image = bookStore.Image(Convert.ToBase64String(model.ElementAt(i).book_image));


                    }
                    return Ok(model);
                }
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Sorting the books based on publication date Old or new
        /// </summary>
        /// <param name="sort"></param>
        /// <returns>Sort Books in Asc/DESC</returns>
        [HttpGet("sort/{sort}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <summary>
        /// To Delete a Book
        /// </summary>
        /// <param name="id"> book id</param>
        /// <returns>status code 204</returns>
        //deleting single book
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <summary>
        /// To Create a New Book
        /// </summary>
        /// <param name="book">Details of the book</param>
        /// <returns>Details of Book</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <summary>
        /// Update The Specific Book
        /// </summary>
        /// <param name="id">Book id</param>
        /// <param name="book"> Details to be updated</param>
        /// <returns>Updated Book Details</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Models.Books?>> UpdateBook(int id, UpdateBook book)
        {
            Console.WriteLine(book);
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
                    bookCurrent.offerOfferId= book.offerOfferId;
                    bookCurrent.AuthorAuthorId = book.AuthorAuthorId;
                    bookCurrent.GenreGenreId = book.GenreGenreId;
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
