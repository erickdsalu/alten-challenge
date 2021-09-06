using Domain.Models;
using Extensions.Paging;
using Persistence.Models;
using System.Linq;

namespace Persistence.Mappers
{
    public static class ReservationsMapper
    {
        public static ReservationPersistence AsPersistence(this Reservation reservation)
        {
            if (reservation is null)
                return null;
            return new ReservationPersistence
            {
                ReservationId = reservation.ReservationId,
                CustomerId = reservation.CustomerId,
                RoomId = reservation.RoomId,
                Status = reservation.Status,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                IsActive = reservation.IsActive
            };
        }

        public static PageModel<Reservation> AsDomainModel(this PageModel<ReservationPersistence> reservation)
        {
            return new PageModel<Reservation>
            {
                LastIndex = reservation.LastIndex,
                Items = reservation.Items.Select(AsDomainModel)
            };
        }

        public static Reservation AsDomainModel(this ReservationPersistence reservation)
        {
            if (reservation is null)
                return null;
            return new Reservation(reservationId: reservation.ReservationId, customerId: reservation.CustomerId, roomId: reservation.RoomId, status: reservation.Status, startDate: reservation.StartDate, endDate: reservation.EndDate, isActive: reservation.IsActive);
        }
    }
}
