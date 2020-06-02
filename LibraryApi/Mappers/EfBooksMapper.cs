using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace LibraryApi.Mappers
{
    public class EfBooksMapper : IMapBooks
    {
        LibraryDataContext Context;
        IMapper Mapper;
        MapperConfiguration Config;

        public EfBooksMapper(LibraryDataContext context, IMapper mapper, MapperConfiguration config)
        {
            Context = context;
            Mapper = mapper;
            Config = config;
        }

        public async Task<GetABookResponse> AddBook(PostBookCreate bookToAdd)
        {
            var book = Mapper.Map<Book>(bookToAdd);
            Context.Books.Add(book); // I have no Id!
            await Context.SaveChangesAsync(); // Suddenly I have an ID! 

            var response = Mapper.Map<GetABookResponse>(book);
            return response;
        }

        public async Task<GetBooksResponse> GetAllBooksFor(string genre)
        {
            var books = Context.Books
                .Where(b => b.InStock)
                .ProjectTo<GetBooksResponseItem>(Config)
                .AsNoTracking();  // On read-only things, ALWAYS do this.

            if (genre != null)
            {
                books = books.Where(b => b.Genre == genre);
            }

            var booksList = await books.ToListAsync();
            var response = new GetBooksResponse
            {
                Books = booksList,
                GenreFilter = genre,
                NumberOfBooks = booksList.Count
            };
            return response;
        }
    }
}
