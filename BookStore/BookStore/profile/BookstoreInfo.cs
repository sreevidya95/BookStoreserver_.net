using AutoMapper;

namespace BookStore.profile
{
    public class BookstoreInfo:Profile
    {
        public BookstoreInfo() {

            CreateMap<Entities.Books, Models.Books>();
            CreateMap<Entities.Genre, Models.Genre>().ReverseMap();

        }
    }
}
