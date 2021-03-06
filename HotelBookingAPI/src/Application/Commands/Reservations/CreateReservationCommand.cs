using MediatR;
using System;

namespace Application.Commands.Reservations
{
    public class CreateReservationCommand : IRequest<Unit>
    {
        public Guid CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
