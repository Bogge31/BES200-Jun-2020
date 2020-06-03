using LibraryApi.Domain;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class ReservationsController : ControllerBase
    {
        LibraryDataContext Context;
        ISendReservationsToTheQueue Queue;

        public ReservationsController(LibraryDataContext context, ISendReservationsToTheQueue queue)
        {
            Context = context;
            Queue = queue;
        }

        [HttpPost("/reservations")]
        public async Task<ActionResult<Reservation>> AddReservation([FromBody] Reservation reservationToAdd)
        {
            var numberOfBooksInReservation = reservationToAdd.Books.Count(c => c == ',');
            //for (var t = 0; t < numberOfBooksInReservation; t++)
            //{
            //    await Task.Delay(1000); // take a second to prcess each book
            //}
            reservationToAdd.ReverationCreated = DateTime.Now;
            reservationToAdd.Status = ReservationStatus.Processing;
            Context.Reservations.Add(reservationToAdd);
            await Context.SaveChangesAsync();
            Queue.SendReservationForProcessing(reservationToAdd);
            return CreatedAtRoute("reservations#get", new { id = reservationToAdd.Id, }, reservationToAdd);
        }

        [HttpGet("/reservations/{id:int}", Name = "reservations#get")]
        public async Task<ActionResult<Reservation>> GetAReservation(int id)
        {
            var reservation = await Context.Reservations.SingleOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(reservation);
            }
        }

        [HttpGet("/reservations/pending")]
        public async Task<ActionResult<List<Reservation>>> GetPendingReservations()
        {
            var result = await Context.Reservations.Where(r => r.Status == ReservationStatus.Processing).ToListAsync();
            return Ok(result);
        }

        [HttpGet("/reservations/approved")]
        public async Task<ActionResult<List<Reservation>>> GetApprovedReservations()
        {
            var result = await Context.Reservations.Where(r => r.Status == ReservationStatus.Approved).ToListAsync();
            return Ok(result);
        }

        [HttpGet("/reservations/denied")]
        public async Task<ActionResult<List<Reservation>>> GetDeniedReservations()
        {
            var result = await Context.Reservations.Where(r => r.Status == ReservationStatus.Denied).ToListAsync();
            return Ok(result);
        }

        [HttpPost("/reservations/approved")]
        public async Task<ActionResult> ApproveReservation([FromBody] Reservation reservation)
        {
            var storedReservation = await Context.Reservations.Where(r => r.Id == reservation.Id).SingleAsync();
            storedReservation.Status = ReservationStatus.Approved;
            await Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("/reservations/denied")]
        public async Task<ActionResult> DenyReservation([FromBody] Reservation reservation)
        {
            var storedReservation = await Context.Reservations.Where(r => r.Id == reservation.Id).SingleAsync();
            storedReservation.Status = ReservationStatus.Denied;
            await Context.SaveChangesAsync();
            return NoContent();
        }
    }
}
