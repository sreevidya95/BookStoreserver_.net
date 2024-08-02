using AutoMapper;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookStoreRepository bookStore;
        private readonly IMapper mapper;

        public BooksController(IBookStoreRepository bookStore,IMapper mapper) {
            this.bookStore = bookStore;
            this.mapper = mapper;
        }
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
        [HttpGet("/{id}")]
        public async Task<ActionResult<Models.Books?>> GetBooksById(int id)
        {
            try
            {
                var book = await bookStore.GetBooksByIdAsync(id);
                if (book == null)
                {
                    return NotFound("Couldnt find the book");
                }
                else
                {

                    return Ok(mapper.Map<Models.Books>(book));
                }
            } catch (Exception ex) { 
            
                  return BadRequest(ex.Message );
            }
        }
        
    }
}
