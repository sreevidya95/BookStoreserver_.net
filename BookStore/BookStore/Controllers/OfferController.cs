using AutoMapper;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IBookStoreRepository bookStore;
        private readonly IMapper mapper;
        

        public OfferController(IBookStoreRepository bookStore, IMapper mapper)
        {
            this.bookStore = bookStore;
            this.mapper = mapper;
           
        }
        [HttpPost]
        public async Task<ActionResult<Models.Offer_Model>> CreateOffer(Models.Offer_Model offer)
        {
            try
            {
                var offerEntity = mapper.Map<Entities.Offer>(offer);
                await bookStore.CreateOffer(offerEntity);
                bool created = await bookStore.SyncDb();
                if (created)
                {
                    for(int i = 0; i < offer.books.Length; i++)
                    {
                        var book = await bookStore.GetBooksByIdAsync(offer.books[i]);
                        book.offerOfferId = offerEntity.offer_id;
                        bool update = await bookStore.SyncDb();
                    }
                    var offerModel = mapper.Map<Models.Offer_Model>(offerEntity);
                     return Ok(offerModel);
                }
                else
                {
                    return Ok("One offer is already going on");
                }

            }
            catch (Exception e) { 
                  return BadRequest(e.Message);
            }
        }
    }
}
