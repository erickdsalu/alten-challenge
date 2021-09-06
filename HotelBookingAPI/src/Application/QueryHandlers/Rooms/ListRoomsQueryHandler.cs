using Application.Mappers;
using Application.Models;
using Application.Queries.Rooms;
using Extensions.Exceptions;
using Extensions.Paging;
using MediatR;
using Persistence.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.QueryHandlers.Rooms
{
    public class ListRoomsQueryHandler : IRequestHandler<ListRoomsQuery, PageModel<RoomModel>>
    {
        private readonly IRoomsRepository _roomsRepository;
        private readonly IReservationsRepository _reservationsRepository;

        public ListRoomsQueryHandler(IRoomsRepository roomsRepository, IReservationsRepository reservationsRepository)
        {
            _roomsRepository = roomsRepository;
            _reservationsRepository = reservationsRepository;
        }

        public async Task<PageModel<RoomModel>> Handle(ListRoomsQuery request, CancellationToken cancellationToken)
        {
            if (request.OnlyAvailableRooms && !(request.StartDate.HasValue && request.EndDate.HasValue))
                throw new CustomNotificationException(HttpStatusCode.BadRequest, $"StartDate or EndDate is required for OnlyAvailableRooms searching");

            if ((request.StartDate.HasValue || request.EndDate.HasValue) && await _reservationsRepository.ListReservations(request) is var reservations)
                request.ExcludentIds = reservations.Items.Select(x => x.RoomId.ToString()).ToArray();

            var rooms = await _roomsRepository.ListRooms(request);

            return rooms.AsApplicationModel();
        }
    }
}
