 using AutoMapper;
using BookStore.Entities;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;

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

                    return Ok(mapper.Map<IEnumerable<Models.Author>>(authors));  
                    
                    }
                       
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            [HttpGet("{id}",Name ="getAuthor")]
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
            [HttpPost]
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
           [HttpPut("{id}")]
           public async Task<ActionResult> UpdateAuthor(int id, UpdateAuthor author)
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
                        mapper.Map(author,authorCurrent);
                        bool updated = await bookStore.SyncDb();
                        if (updated == true)
                        {
                                logger.LogInformation($"The Author with id '{authorCurrent.author_id}' is updated");
                                return Ok("Updated Successfully");
                        }
                        else
                        {
                            return Ok("seems like no changes are made to update");
                        }
                    }
                }catch(Exception ex)
                {
                return BadRequest(ex.Message);
                }
           }

        }
}

