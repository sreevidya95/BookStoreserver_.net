 using AutoMapper;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{ 
        [Route("[Controller]")]
        [ApiController]
        public class AuthorController : ControllerBase
    {
            private readonly IBookStoreRepository bookStore;
            private readonly IMapper mapper;
            private readonly ILogger<AuthorController> logger;

            public AuthorController(IBookStoreRepository bookStore, IMapper mapper, ILogger<AuthorController> logger)
            {
                this.bookStore = bookStore;
                this.mapper = mapper;
                this.logger = logger;

            }
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Models.Author>>> GetAuthors()
            {
                try
                {
                    var authors = await bookStore.GetAuthorAsync();
                    if (authors == null)
                    {
                        return NotFound("NO Authors Available");
                    }
                    else
                    {
                        return Ok(mapper.Map<IEnumerable<Entities.Author>>(authors));
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            [HttpGet("{id}")]
            public async Task<ActionResult<Models.Author?>> getAuthors(int id)
            {
                try
                {
                    var author = await bookStore.GetAuthorByIdAsync(id);
                    if (author == null)
                    {
                        return NotFound("Couldnt find the Author for the given id");
                    }
                    else
                    {
                        return Ok(mapper.Map<Models.Author>(author));
                    }
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteAuthor(int id)
            {
                try
                {
                    await bookStore.DeleteAuthorAsync(id);
                    bool deleted = await bookStore.SyncDb();
                    if (deleted == true)
                    {
                        logger.LogWarning($"The Author with id '{id}' is deleted");
                         return NoContent();
                    }
                    else
                    {
                        return StatusCode(405,"Seems like author have one or more books");
                    }

                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }
           //Need to implement post and put

        }
}

