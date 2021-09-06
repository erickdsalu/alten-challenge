using Application.Models;
using Extensions.Paging;
using System.Linq;
using Web.ApiModels.Responses.Reservations;

namespace Web.Mappers
{
    public static class ReservationsMapper
    {
        public static PageModel<GetReservationResponse> AsResponseModel(this PageModel<ReservationModel> reservation)
        {
            return new PageModel<GetReservationResponse>
            {
                LastIndex = reservation.LastIndex,
                Items = reservation.Items.Select(AsResponseModel)
            };
        }

        public static GetReservationResponse AsResponseModel(this ReservationModel reservationModel)
        {
            return new GetReservationResponse
            {
                Id = reservationModel.Id,
                CustomerId = reservationModel.CustomerId,
                RoomId = reservationModel.RoomId,
                Status = reservationModel.Status,
                StartDate = reservationModel.DateStart,
                EndDate = reservationModel.DateEnd
            };
        }
    }
}
