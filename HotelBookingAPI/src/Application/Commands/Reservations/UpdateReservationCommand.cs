using MediatR;
using System;

namespace Application.Commands.Reservations
{
    public class UpdateReservationCommand : IRequest<Unit>
    {
        public Guid CustomerId { get; set; }
        public Guid ReservationId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
