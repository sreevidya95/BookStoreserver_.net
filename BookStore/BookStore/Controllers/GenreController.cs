using AutoMapper;
using BookStore.Entities;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.Controllers
{
    /// <summary>
    /// Genre Controller
    /// </summary>
    [Route("[Controller]")]
    [ApiController]
    [Authorize]
    public class GenreController : ControllerBase
    {
        private readonly IBookStoreRepository bookStore;
        private readonly IMapper mapper;
        private readonly ILogger<GenreController> logger;
        /// <summary>
        /// Genre Controller Constructor
        /// </summary>
        /// <param name="bookStore">dependency Injection for IbboRepo</param>
        /// <param name="mapper"> for automapper</param>
        /// <param name="logger">for serilog</param>
        public GenreController(IBookStoreRepository bookStore, IMapper mapper, ILogger<GenreController> logger)
        {
            this.bookStore = bookStore;
            this.mapper = mapper;
            this.logger = logger;
        }
        //getting all genres from database
        /// <summary>
        /// Get All Genres for Database
        /// </summary>
        /// <returns>all Genres</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <summary>
        /// Getting specific genre
        /// </summary>
        /// <param name="id">genre id</param>
        /// <returns>specifi genre</returns>
        //getting specific genre
        [HttpGet("{id}",Name ="getGenre")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <summary>
        /// Create New Genre
        /// </summary>
        /// <param name="genre">Genre NAme</param>
        /// <returns>newly create genre</returns>
        //adding specific genre
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                    logger.LogInformation($"added Genere {newGenreModel.genre_name}");
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
