using AutoMapper;

namespace BookStore.profile
{
    public class BookstoreInfo:Profile
    {
        public BookstoreInfo() {

            CreateMap<Entities.Books, Models.Books>().ReverseMap();
            CreateMap<Entities.Genre, Models.Genre>().ReverseMap();
            CreateMap<Entities.Author, Models.Author>().ReverseMap();
            CreateMap<Entities.Admin, Models.AdminEmail>();
            CreateMap<Entities.Admin, Models.Admin>().ReverseMap();

        }
    }
}
