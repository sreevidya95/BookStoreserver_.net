using AutoMapper;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookStore.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class Enquiry : ControllerBase
    {
        private readonly IBookStoreRepository bookStore;
        private readonly IMapper mapper;

        public Enquiry(IBookStoreRepository bookStore,IMapper mapper)
        {
            this.bookStore = bookStore;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult> CreateMessage(Models.Entity enquiry)
        {
            try
            {
                var NewEnquiry = mapper.Map<Entities.Enquiry>(enquiry);
                  await bookStore.CreateEnquiry(NewEnquiry);
                bool created = await bookStore.SyncDb();
                if (created == true)
                {
                    return Ok("Enquiry Created Successfully");
                }
                else
                {
                    return NotFound("Couldnt Send this Enquiry");
                }
            }
            catch (Exception ex) { 
                  return BadRequest(ex.Message);
            }
        }
        [HttpGet]
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
        [HttpDelete("{id}")]
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
        [HttpPut("{id}")]
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
                        return Ok("updated Successfully");
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
