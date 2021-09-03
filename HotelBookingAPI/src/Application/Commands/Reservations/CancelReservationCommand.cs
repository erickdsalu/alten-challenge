using MediatR;
using System;

namespace Application.Commands.Reservations
{
    public class CancelReservationCommand : IRequest<Unit>
    {
        public Guid ReservationId { get; set; }
        public Guid RoomId { get; set; }
    }
}
