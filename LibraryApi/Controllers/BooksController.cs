using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Filters;
using LibraryApi.Mappers;
using LibraryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class BooksController : Controller
    {
        LibraryDataContext Context;
        IMapBooks Mapper;

        public BooksController(LibraryDataContext context, IMapBooks mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        // Jeff says this is really cool. Maybe look at it again some day.
        [HttpPut("books/{id:int}/numberofpages")]
        public async Task<ActionResult> ChangeNumberOfPages(int id, [FromBody] int numberOfPages)
        {
            // validate the thing
            if (numberOfPages <= 0)
            {
                return BadRequest("Get a life, loser!");
            }
            var book = await Context.Books
                    .Where(b => b.Id == id && b.InStock)
                    .SingleOrDefaultAsync();

            if (book != null)
            {
                book.NumberOfPages = numberOfPages;
                await Context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("books/{bookId:int}")]
        public async Task<ActionResult> RemoveABook(int bookId)
        {
            var bookToRemove = await Context.Books
                .Where(b => b.InStock && b.Id == bookId)
                .SingleOrDefaultAsync();

            if (bookToRemove != null)
            {
                // we never delete anything from a database.
                bookToRemove.InStock = false;
                await Context.SaveChangesAsync();
            }
            return NoContent(); // Fine.
        }

        [HttpPost("books")]
        [ValidateModel]
        public async Task<ActionResult> AddABook([FromBody] PostBookCreate bookToAdd)
        {
            GetABookResponse response = await Mapper.AddBook(bookToAdd);

            return CreatedAtRoute("books#getabook", new { bookId = response.Id }, response);
        }

        /// <summary>
        /// Retrieve one of our books
        /// </summary>
        /// <param name="bookId">The Id of the book you want to find</param>
        /// <returns>A book</returns>
        [HttpGet("books/{bookId:int}", Name = "books#getabook")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetABookResponse>> GetABook(int bookId)
        {
            var book = await Context.Books
                .Where(b => b.InStock && b.Id == bookId)
                .Select(b => new GetABookResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    NumberOfPages = b.NumberOfPages
                }).SingleOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }

        [HttpGet("books")]
        public async Task<ActionResult<GetABookResponse>> GetAllBooks([FromQuery] string genre)
        {
            GetBooksResponse response = await Mapper.GetAllBooksFor(genre);

            return Ok(response);
        }
    }
}
