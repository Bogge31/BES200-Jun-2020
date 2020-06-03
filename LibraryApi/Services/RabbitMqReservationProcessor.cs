using LibraryApi.Domain;
using LibraryApi.Migrations;
using RabbitMqUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class RabbitMqReservationProcessor : ISendReservationsToTheQueue
    {
        IRabbitManager Manager;

        public RabbitMqReservationProcessor(IRabbitManager manager)
        {
            Manager = manager;
        }

        public void SendReservationForProcessing(Reservation reservation)
        {
            Manager.Publish(reservation, "", "direct", "reservations");
        }
    }
}
