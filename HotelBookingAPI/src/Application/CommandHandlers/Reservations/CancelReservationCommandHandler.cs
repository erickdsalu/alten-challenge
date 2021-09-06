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
    public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, Unit>
    {
        private readonly IConfigurationsRepository _configurationsRepository;
        private readonly IReservationsRepository _reservationRepository;
        private readonly IRoomsRepository _roomsRepository;

        public CancelReservationCommandHandler(
            IConfigurationsRepository configurationsRepository, IReservationsRepository reservationRepository, IRoomsRepository roomsRepository)
        {
            _reservationRepository = reservationRepository;
            _configurationsRepository = configurationsRepository;
            _roomsRepository = roomsRepository;
        }

        public async Task<Unit> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {           
            var reservation = await _reservationRepository.GetReservation(request.RoomId, request.ReservationId);

            if (reservation is null)
                throw new CustomNotificationException(HttpStatusCode.NotFound,
                    $"Reservation {request.ReservationId} wasn't found");


            if(reservation.RoomId != request.RoomId)
                throw new CustomNotificationException(HttpStatusCode.NotFound,
                    $"Can't change rooms for a reservation, please cancel this reservation and create another one");

            reservation.CancelReservation();
            
            await _reservationRepository.SaveReservation(reservation);

            return Unit.Value;
        }
    }
}
