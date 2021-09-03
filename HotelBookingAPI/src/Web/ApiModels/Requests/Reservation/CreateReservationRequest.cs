using System;

namespace Web.ApiModels.Requests.Reservation
{
    public class CreateReservationRequest
    {
        public Guid CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
