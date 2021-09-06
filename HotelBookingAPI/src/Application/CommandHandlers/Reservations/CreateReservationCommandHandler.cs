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
        private readonly IRoomsRepository _roomsRepository;

        public CreateReservationCommandHandler(
            IConfigurationsRepository configurationsRepository, IReservationsRepository reservationRepository, IRoomsRepository roomsRepository)
        {
            _reservationRepository = reservationRepository;
            _configurationsRepository = configurationsRepository;
            _roomsRepository = roomsRepository;
        }

        public async Task<Unit> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var hotelConfiguration = await _configurationsRepository.GetHotelConfiguration();
            var room = await _roomsRepository.GetRoom(request.RoomId);
            var reservation = Reservation.GenerateReservation(request.CustomerId, request.RoomId, request.StartDate, request.EndDate);

            reservation.ValidateReservation(hotelConfiguration);

            if(room is null)
                throw new CustomNotificationException(HttpStatusCode.NotFound,
                    $"Room {request.RoomId} wasn't found");

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
