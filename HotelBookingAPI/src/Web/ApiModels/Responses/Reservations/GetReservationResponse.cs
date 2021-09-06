using Application.Enums;
using System;

namespace Web.ApiModels.Responses.Reservations
{
    public class GetReservationResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public ReservationStatusModel Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
