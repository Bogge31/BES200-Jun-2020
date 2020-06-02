using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests.Books
{
    public class GettingAllBooks : IClassFixture<WebTestFixture>
    {
        private readonly HttpClient Client;
        public GettingAllBooks(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        // Do we get a 200?
        [Fact]
        public async Task HasOkstatus()
        {
            var response = await Client.GetAsync("/books");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Is it returning Json?
        [Fact]
        public async Task HasJsonResponse()
        {
            var response = await Client.GetAsync("/books");
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }


        // does it have the right data?
        [Fact]
        public async Task HasTheRightData()
        {
            var response = await Client.GetAsync("/books");

            var data = await response.Content.ReadAsAsync<GetBooksResponse>();

            Assert.Equal(2, data.numberOfBooks);

            var firstBook = data.books.Single(b => b.id == 1);
            Assert.Equal("Jaws", firstBook.title);
            Assert.Null(data.genreFilter);
        }

        // If we use a genre, does it filter?
        [Fact]
        public async Task FilteringByGenre()
        {
            var response = await Client.GetAsync("/books?genre=Fantasy");

            var data = await response.Content.ReadAsAsync<GetBooksResponse>();

            Assert.Equal(1, data.numberOfBooks);
            Assert.Equal("Fantasy", data.genreFilter);
        }


    }


    public class GetBooksResponse
    {
        public Book[] books { get; set; }
        public string genreFilter { get; set; }
        public int numberOfBooks { get; set; }
    }

    public class Book
    {
        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public int numberOfPages { get; set; }
    }

}
