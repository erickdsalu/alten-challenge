using Application.Mappers;
using Application.Models;
using Application.Queries.Reservations;
using Extensions.Exceptions;
using MediatR;
using Persistence.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.QueryHandlers.Reservations
{
    public class GetReservationQueryHandler : IRequestHandler<GetReservationQuery, ReservationModel>
    {

        private readonly IReservationsRepository _reservationRepository;

        public GetReservationQueryHandler(IReservationsRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<ReservationModel> Handle(GetReservationQuery request, CancellationToken cancellationToken)
        {
            var result = await _reservationRepository.GetReservation(request.RoomId, request.ReservationId);

            if (result == null)
                throw new CustomNotificationException(HttpStatusCode.NotFound,
                    $"The reservation was't found");

            return result.AsApplicationModel();
        }
    }
}
