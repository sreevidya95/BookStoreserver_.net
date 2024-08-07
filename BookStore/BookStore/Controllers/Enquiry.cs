using AutoMapper;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BookStore.Controllers
{
    /// <summary>
    /// Enquiry Controller
    /// </summary>
    [Route("[Controller]")]
    [ApiController]
    public class Enquiry : ControllerBase
    {
        private readonly IBookStoreRepository bookStore;
        private readonly IMapper mapper;
        /// <summary>
        /// Enquiry Controller Constructor
        /// </summary>
        /// <param name="bookStore">Ibookstorerepo</param>
        /// <param name="mapper">automapper</param>
        public Enquiry(IBookStoreRepository bookStore,IMapper mapper)
        {
            this.bookStore = bookStore;
            this.mapper = mapper;
        }
        /// <summary>
        /// To create new Enquiry
        /// </summary>
        /// <param name="enquiry"></param>
        /// <returns>201 Status code</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Models.Entity?>> CreateMessage(Models.Entity enquiry)
        {
            try
            {
                var NewEnquiry = mapper.Map<Entities.Enquiry>(enquiry);
                  await bookStore.CreateEnquiry(NewEnquiry);
                bool created = await bookStore.SyncDb();
                if (created == true)
                {
                    return Ok(JsonConvert.SerializeObject(new
                    {
                        status = 200,
                        msg = "Enquiry Sent"
                    }));
                    
                }
                else
                {
                    return NotFound(JsonConvert.SerializeObject(new
                    {
                        status = 404,
                        msg = "Couldn't Send this Enquiry"
                    }));
                }
            }
            catch (Exception ex) { 
                  return BadRequest(JsonConvert.SerializeObject(new
                  {
                      status = 500,
                      msg = ex.Message
                  }));
            }
        }
        /// <summary>
        /// Get All Enquiries
        /// </summary>
        /// <returns>all enquiries</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Models.Entity>>>GetAll()
        {
            try
            {
                var enquiries = await bookStore.GetEnquiry();
                if (enquiries != null)
                {
                    return Ok(mapper.Map<IEnumerable<Models.Entity>>(enquiries));
                }
                else
                {
                    return NotFound("No Enquiries Found");
                }
            }
            catch (Exception ex) { 
                   return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Delete an Enquiry
        /// </summary>
        /// <param name="id">enquiry id</param>
        /// <returns>204 status code</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteEnquiry(int id)
        {
            try
            {
                await bookStore.DeleteEnquiry(id);
                bool deleted = await bookStore.SyncDb();
                if (deleted == true)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound("Couldnt deleted");
                }

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update Enquiry as Read
        /// </summary>
        /// <param name="id">enquiry id</param>
        /// <returns>enquiry details</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> UpdateEnquiry(int id)
        {
            try
            {
                var enquiry = await bookStore.GetEnquiryById(id);
                if (enquiry != null)
                {
                    enquiry.isRead = 1;
                    bool update = await bookStore.SyncDb();
                    if (update == true)
                    {
                       var Enquiry = await bookStore.GetEnquiryById(id);
                        
                        return Ok(mapper.Map<Models.Entity?>(Enquiry));
                    }
                    else
                    {
                        return BadRequest("Something went wrong");
                    }
                }
                else
                {
                    return NotFound("No Enquiry found");
                }
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            
            }
        }
    }
}
