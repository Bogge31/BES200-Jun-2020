using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class ResourceSmokeTests:IClassFixture<WebTestFixture>
    {
        private readonly HttpClient Client;

        public ResourceSmokeTests(WebTestFixture fixture)
        {
            Client = fixture.CreateClient();
        }

        [Theory]
        [InlineData("books")]
        [InlineData("status")]
        public async Task IsAlive(string resource)
        {
            var response =await Client.GetAsync(resource);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
