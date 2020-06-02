using LibraryApi.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApiIntegrationTests
{
    public static class DataUtils
    {
        public static void Initialize(LibraryDataContext db)
        {
            db.Books.AddRange(
                GetSeedingBooks()
                );
            db.SaveChanges();
        }

        public static void ReInitializeDb(LibraryDataContext db)
        {
            db.Books.RemoveRange(db.Books); // removes all the books
            Initialize(db);
        }

        public static List<Book> GetSeedingBooks()
        {
            return new List<Book>
            {
                new Book { Id = 1, Title = "Jaws", Author = "Benchely", Genre = "Fiction", InStock = true, NumberOfPages = 200 },
                new Book { Id = 2, Title = "Title 2", Author = "Smith", Genre = "Fantasy", InStock = true, NumberOfPages = 321 },
                new Book { Id = 3, Title = "Jaws", Author = "Benchely", Genre = "Fiction", InStock = false, NumberOfPages = 200 }
            };
        }
    }
}
