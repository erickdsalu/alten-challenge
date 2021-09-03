using Application.Models;
using Extensions.Paging;
using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Queries.Reservations
{
    public class ListReservationsQuery : PagingRequest, IRequest<PageModel<ReservationModel>>
    {
        public Guid? RoomId { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
