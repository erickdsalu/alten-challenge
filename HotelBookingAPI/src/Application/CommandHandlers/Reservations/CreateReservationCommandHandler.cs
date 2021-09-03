using Application.Commands.Reservations;
using Domain.Models;
using Extensions.Exceptions;
using Extensions.Paging;
using MediatR;
using Persistence.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CommandHandlers.Reservations
{
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Unit>
    {
        private readonly IConfigurationsRepository _configurationsRepository;
        private readonly IReservationsRepository _reservationRepository;

        public CreateReservationCommandHandler(
            IConfigurationsRepository configurationsRepository, IReservationsRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
            _configurationsRepository = configurationsRepository;
        }

        public async Task<Unit> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var hotelConfiguration = await _configurationsRepository.GetHotelConfiguration();
            var reservation = Reservation.GenerateReservation(request.CustomerId, request.RoomId, request.StartDate, request.EndDate);


            reservation.ValidateReservation(hotelConfiguration);

            var reservationsAtSameInterval = await _reservationRepository.ListReservationsByRoom(
                new PagingRequest
                {
                    StartDate = request.StartDate,
                    EndDate = request.EndDate
                }, request.RoomId);

            if (reservationsAtSameInterval.Count > 0)
                throw new CustomNotificationException(HttpStatusCode.Conflict,
                    $"A reservation at the same interval was already requested to the room {request.RoomId}");

            await _reservationRepository.SaveReservation(reservation);

            return Unit.Value;
        }
    }
}
