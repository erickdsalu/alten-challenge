using Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries.Reservations
{
    public class GetReservationQuery : IRequest<ReservationModel>
    {
        public Guid RoomId { get; set; }

        public Guid ReservationId { get; set; }
    }
}
