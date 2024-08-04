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
                    
                    return Ok(mapper.Map<IEnumerable<Models.Books>>(books));
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

                    return Ok(mapper.Map<Models.Books>(book));
                }
            } catch (Exception ex) { 
            
                  return BadRequest(ex.Message );
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

                var bookEntity = mapper.Map<Entities.Books>(book);

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
        //not working
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, UpdateBook book)
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
                    mapper.Map(book,bookCurrent);
                    bool updated = await bookStore.SyncDb();
                    if (updated == true)
                    {
                        log.LogInformation($"The Book with id '{bookCurrent.book_id}' is updated");
                        return Ok("Book Updated Successfully");
                    }
                    else
                    {
                        return Ok("seems like no changes are made to update");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
