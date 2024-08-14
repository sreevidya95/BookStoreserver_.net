 using AutoMapper;
using BookStore.Entities;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.Controllers
{ 
    /// <summary>
    /// Author Controller
    /// </summary>
        [Route("[Controller]")]
        [ApiController]
        [Authorize]
        public class AuthorController : ControllerBase
    {
            private readonly IBookStoreRepository bookStore;
            private readonly IMapper mapper;
            private readonly ILogger<AuthorController> logger;
        /// <summary>
        /// Author Controller Constructor
        /// </summary>
        /// <param name="bookStore">dependency Injection of Ibook repo</param>
        /// <param name="mapper"> for automapper</param>
        /// <param name="logger"> for serilog</param>
            public AuthorController(IBookStoreRepository bookStore, IMapper mapper, ILogger<AuthorController> logger)
            {
                this.bookStore = bookStore;
                this.mapper = mapper;
                this.logger = logger;

            }
            /// <summary>
            /// Get all Authors From Database
            /// </summary>
            /// <returns> Returns all authors</returns>
                [HttpGet]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status502BadGateway)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

                        var authorEntity = mapper.Map<IEnumerable<Models.Author>>(authors);
                        for (int i = 0; i < authorEntity.Count(); i++)
                        {
                            if (authorEntity.ElementAt(i).author_image != null)
                                authorEntity.ElementAt(i).author_image = bookStore.Image(Convert.ToBase64String(authorEntity.ElementAt(i).author_image));


                        }
                            return Ok(authorEntity);

                    }
                       
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
           /// <summary>
           /// Specifi Author
           /// </summary>
           /// <param name="id">Author Id</param>
           /// <returns>Return a specific book</returns>
            [HttpGet("{id}",Name ="getAuthor")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status502BadGateway)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                       var authorEntity = mapper.Map<Models.Author>(author);
                            if (author.author_image != null)
                            {
                                authorEntity.author_image = bookStore.Image(Convert.ToBase64String(author.author_image));
                            }
                            return Ok(authorEntity);
                }
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }
        /// <summary>
        /// Delete Particular Author
        /// </summary>
        /// <param name="id">Author id to delete</param>
        /// <returns>204 status</returns>
            [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
             /// <summary>
             /// Create Author
             /// </summary>
             /// <param name="author">author details</param>
             /// <returns>newly created author details</returns>
            [HttpPost]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status502BadGateway)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Models.Author>>CreateAuthor(Models.UpdateAuthor author)
            {
                try
                {

                var authorEntity = new Entities.Author
                {
                    name = author.name,
                    biography = author.biography

                };
                if (author.author_image != null)
                {
                    var file = author.author_image;
                    //read the file
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        var fileBytes = memoryStream.ToArray();
                        authorEntity.author_image = fileBytes;
                    }
                }

                await bookStore.CreateAuthor(authorEntity);
                    bool added = await bookStore.SyncDb();
                    if (added == true)
                    {
                   
                       var authorModel = mapper.Map<Models.Author>(authorEntity);
                       logger.LogInformation($"The Author '{authorModel.name}' is added");
                        return CreatedAtRoute("getAuthor", new
                        {
                            id = authorModel.author_id
                        }, authorModel);
                   
                    }
                    else
                    {
                        return Ok("Author already Exists");
                    }

                }catch(Exception e)
                {
                    return BadRequest(e.Message);
                }

            }
          /// <summary>
          /// Update anAuthor
          /// </summary>
          /// <param name="id">Details to Update</param>
          /// <param name="author">Return Updated Author</param>
          /// <returns></returns>
           [HttpPut("{id}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status502BadGateway)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Models.Author?>> UpdateAuthor(int id, UpdateAuthor author)
           {
                try
                {
                var authorCurrent = await bookStore.GetAuthorByIdAsync(id);
                    if (authorCurrent == null)
                    {
                        return NotFound("Invalid Author");
                    }
                    else
                    {
                    if (author.author_image != null)
                    {
                        var file = author.author_image;
                        //read the file
                        using (var memoryStream = new MemoryStream())
                        {
                            file.CopyTo(memoryStream);
                            var fileBytes = memoryStream.ToArray();
                            authorCurrent.author_image = fileBytes;
                        }
                    }
                    authorCurrent.name = author.name;
                    authorCurrent.biography = author.biography;
                    authorCurrent.UpdatedAt = author.UpdatedAt;

                    bool updated = await bookStore.SyncDb();
                    var model = mapper.Map<Models.Author>(authorCurrent);
                    if (updated == true)
                        {
                           logger.LogInformation($"The Author with id '{authorCurrent.author_id}' is updated");
                               
                        }
                    
                    return Ok(model);
                }
                }catch(Exception ex)
                {
                return BadRequest(ex.Message);
                }
           }

        }
}

