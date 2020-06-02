using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class CatalogService : ICacheTheCatalog
    {
        IDistributedCache Cache;

        public CatalogService(IDistributedCache cache)
        {
            Cache = cache;
        }

        public async Task<CatalogModel> GetCatalogAsync()
        {
            var catalog = await Cache.GetAsync("catalog");
            string newCatalog = null;
            if (catalog == null)
            {
                newCatalog = $"This catalog was created at {DateTime.Now.ToLongTimeString()}";
                var encodedCatalog = Encoding.UTF8.GetBytes(newCatalog);
                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddSeconds(15));
                await Cache.SetAsync("catalog", encodedCatalog, options);
            }
            else
            {
                newCatalog = Encoding.UTF8.GetString(catalog);
            }
            return new CatalogModel { Data = newCatalog };
        }
    }

    public class CatalogModel
    {
        public string Data { get; set; }
    }
}
