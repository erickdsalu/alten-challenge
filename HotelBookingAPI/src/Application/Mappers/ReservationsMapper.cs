using Application.Models;
using Domain.Models;
using Extensions.Paging;
using System.Linq;

namespace Application.Mappers
{
    public static class ReservationsMapper
    {

        public static PageModel<ReservationModel> AsApplicationModel(this PageModel<Reservation> reservation)
        {
            return new PageModel<ReservationModel>
            {
                LastIndex = reservation.LastIndex,
                Items = reservation.Items.Select(AsApplicationModel)
            };
        }

        public static ReservationModel AsApplicationModel(this Reservation reservation)
        {
            if (reservation is null)
                return null;
            return new ReservationModel
            {
                Id = reservation.ReservationId,
                CustomerId = reservation.CustomerId,
                RoomId = reservation.RoomId,
                Status = (Enums.ReservationStatusModel)reservation.Status,
                DateStart = reservation.StartDate,
                DateEnd = reservation.EndDate
            };
        }
    }
}
