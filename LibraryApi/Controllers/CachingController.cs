using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using LibraryApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    public class CachingController : ControllerBase
    {
        ICacheTheCatalog Catalog;

        public CachingController(ICacheTheCatalog catalog)
        {
            Catalog = catalog;
        }

        [HttpGet("/time")]
        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any)]
        public ActionResult GetTime()
        {
            return Ok(new { data = $"The time is now {DateTime.Now.ToLongTimeString()}" });
        }

        [HttpGet("/catalog")]
        public async Task<ActionResult> GetCatalog()
        {
            var response = await Catalog.GetCatalogAsync();
            return Ok(response);
        }
    }
}
