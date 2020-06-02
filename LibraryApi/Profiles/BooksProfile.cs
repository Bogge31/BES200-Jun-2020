using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Book, GetBooksResponseItem>();

            // PostBookCreate -> Book
            CreateMap<PostBookCreate, Book>()
                .ForMember(dest => dest.InStock, d => d.MapFrom((_) => true));

            CreateMap<Book, GetABookResponse>();
        }
    }
}
