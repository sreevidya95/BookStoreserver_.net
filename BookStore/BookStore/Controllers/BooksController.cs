using AutoMapper;
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
        [HttpGet("{id}")]
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
        public async Task<ActionResult> DeleteBook(int id)
        {
            try
            {
                await bookStore.DeleteBooksAsync(id);
                bool changes = await bookStore.SyncDb();
                if (changes == true)
                {
                    log.LogInformation($"A book with id '{id}' is delete");
                    return Ok("Deleted Successfully");
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
        //have to implement add book and update book
        
    }
}
