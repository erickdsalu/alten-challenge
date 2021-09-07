using Application.Mappers;
using Application.Models;
using Application.Queries.Reservations;
using Domain.Models;
using Extensions.Exceptions;
using Extensions.Paging;
using MediatR;
using Persistence.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.QueryHandlers.Reservations
{
    public class ListReservationsQueryHandler : IRequestHandler<ListReservationsQuery, PageModel<ReservationModel>>
    {

        private readonly IReservationsRepository _reservationRepository;

        public ListReservationsQueryHandler(IReservationsRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<PageModel<ReservationModel>> Handle(ListReservationsQuery request, CancellationToken cancellationToken)
        {
            PageModel<Reservation> reservations;

            if (request.RoomId.HasValue)
                reservations = await _reservationRepository.ListReservationsByRoom(request, request.RoomId.Value);
            else if (request.CustomerId.HasValue)
                reservations = await _reservationRepository.ListReservationsByCustomer(request, request.CustomerId.Value);
            else
                reservations = await _reservationRepository.ListReservations(request);

            if (reservations.Count == 0)
                throw new CustomNotificationException(HttpStatusCode.NotFound,
                    $"There was no reservations found for your search criteria");

            return reservations.AsApplicationModel();
        }
    }
}
