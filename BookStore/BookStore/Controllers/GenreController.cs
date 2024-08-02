using AutoMapper;
using BookStore.Entities;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IBookStoreRepository bookStore;
        private readonly IMapper mapper;
        private readonly ILogger<GenreController> logger;

        public GenreController(IBookStoreRepository bookStore, IMapper mapper, ILogger<GenreController> logger)
        {
            this.bookStore = bookStore;
            this.mapper = mapper;
            this.logger = logger;
        }
        //getting all genres from database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Genre>>> GetGenres()
        {
            try
            {
                var genres = await bookStore.GetGenresAsync();
                if (genres == null)
                {
                    return NotFound("No Genres Found");
                }
                else
                {
                    return Ok(mapper.Map<IEnumerable<Models.Genre>>(genres));

                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        //getting specific genre
        [HttpGet("{id}",Name ="getGenre")]
        public async Task<ActionResult<Models.Genre?>> getGenreById(int id)
        {
            try
            {
                var genre = await bookStore.GetGenresByIdAsync(id);
                if (genre == null)
                {
                    return NotFound("No Genre Found for the requested id");
                }
                else
                {
                    return Ok(mapper.Map<Models.Genre>(genre));
                }
            }
            catch (Exception ex) { 
                   return BadRequest(ex.Message);
            }
        }
        //adding specific genre
        [HttpPost]
        public async Task<ActionResult<Models.Genre>> CreateGenre(Models.Genre genre)
        {
            try
            {

                var newGenre = mapper.Map<Entities.Genre>(genre);
                await bookStore.CreateGenre(newGenre);
                    bool value = await bookStore.SyncDb();       
                
                if (value == true)
                    {
                       var newGenreModel = mapper.Map<Models.Genre>(newGenre);
                    return CreatedAtRoute("getGenre", new
                    {
                        id = newGenreModel.genre_id
                    }, newGenreModel);
                    }
                    else
                    {
                        return BadRequest("Couldnt Create Genre");
                    }
            }
            catch (Exception ex) {

                return BadRequest(ex.Message);
            }
        }

    }
}
