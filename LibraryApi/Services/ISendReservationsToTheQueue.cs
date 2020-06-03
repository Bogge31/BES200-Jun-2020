using LibraryApi.Domain;

namespace LibraryApi.Services
{
    public interface ISendReservationsToTheQueue
    {
        void SendReservationForProcessing(Reservation reservation);
    }
}