using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApi
{
    public static class MigrateDatabaseExtensions
    {
        public static IHost MigrateDatabase<T>(this IHost webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                T db = null;
                var services = scope.ServiceProvider;
                try
                {
                    db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred trying to migrate the database");
                    try
                    {
                        Thread.Sleep(5000);
                        db.Database.Migrate();
                    }
                    catch (Exception ex2)
                    {
                        logger.LogError(ex2, "Even waiting 5 seconds didn't help to be able to migrate");
                    }
                }
            }

            return webHost;
        }
    }
}
