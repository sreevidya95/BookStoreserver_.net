using AutoMapper;
using BookStore.Models;

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
            CreateMap<Models.UpdateAuthor,Entities.Author>();
            CreateMap<Models.UpdateBook,Entities.Books>();
            CreateMap<Models.Entity, Entities.Enquiry>().ReverseMap();

        }
    }
}
