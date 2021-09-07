using Application.Commands.Reservations;
using Domain.Models;
using Extensions.Exceptions;
using Extensions.Paging;
using MediatR;
using Persistence.Interfaces;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CommandHandlers.Reservations
{
    public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, Unit>
    {
        private readonly IConfigurationsRepository _configurationsRepository;
        private readonly IReservationsRepository _reservationRepository;
        private readonly IRoomsRepository _roomsRepository;

        public UpdateReservationCommandHandler(
            IConfigurationsRepository configurationsRepository, IReservationsRepository reservationRepository, IRoomsRepository roomsRepository)
        {
            _reservationRepository = reservationRepository;
            _configurationsRepository = configurationsRepository;
            _roomsRepository = roomsRepository;
        }

        public async Task<Unit> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            var hotelConfiguration = await _configurationsRepository.GetHotelConfiguration();

            var reservation = await _reservationRepository.GetReservation(request.RoomId, request.ReservationId);

            if (reservation is null)
                throw new CustomNotificationException(HttpStatusCode.NotFound,
                    $"Reservation {request.ReservationId} wasn't found");


            if(reservation.RoomId != request.RoomId)
                throw new CustomNotificationException(HttpStatusCode.NotFound,
                    $"Can't change rooms for a reservation, please cancel this reservation and create another one");

            reservation.UpdateReservation(hotelConfiguration, request.StartDate, request.EndDate);

            var reservationsAtSameInterval = await _reservationRepository.ListReservationsByRoom(
                new PagingRequest
                {
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                }, request.RoomId);

            if (reservationsAtSameInterval?.Items?.Where(x => x.CustomerId != request.CustomerId).Count() > 0)
                throw new CustomNotificationException(HttpStatusCode.Conflict,
                    $"A reservation at the same interval was already requested to another user at the room {request.RoomId}");

            await _reservationRepository.SaveReservation(reservation);

            return Unit.Value;
        }
    }
}
