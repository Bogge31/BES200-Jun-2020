using LibraryApi.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class ReservationsController : ControllerBase
    {
        LibraryDataContext Context;

        public ReservationsController(LibraryDataContext context)
        {
            Context = context;
        }

        [HttpPost("/reservations")]
        public async Task<ActionResult<Reservation>> AddReservation([FromBody] Reservation reservationToAdd)
        {
            var numberOfBooksInReservation = reservationToAdd.Books.Count(c => c == ',');
            for (var t = 0; t < numberOfBooksInReservation; t++)
            {
                await Task.Delay(1000); // take a second to prcess each book
            }
            reservationToAdd.ReverationCreated = DateTime.Now;
            reservationToAdd.Status = ReservationStatus.Processing;
            Context.Reservations.Add(reservationToAdd);
            await Context.SaveChangesAsync();
            return Ok(reservationToAdd);
        }
    }
}
