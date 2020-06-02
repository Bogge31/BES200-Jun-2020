using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Domain
{
    public class LibraryDataContext : DbContext
    {
        public LibraryDataContext(DbContextOptions<LibraryDataContext> ctx) : base(ctx) { }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Property(b => b.Title).HasMaxLength(200);
            modelBuilder.Entity<Book>().Property(b => b.Author).HasMaxLength(200);
            // etc. etc.
        }
    }

    public enum ReservationStatus { Processing, Approved, Denied }

    public class Reservation
    {
        public int Id { get; set; }
        public string For { get; set; }
        public string Books { get; set; } //"1,3,8,16
        public ReservationStatus Status { get; set; }
        public DateTime ReverationCreated { get; set; }
    }
}
