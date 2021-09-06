using Application.Models;
using Extensions.Paging;
using MediatR;

namespace Application.Queries.Rooms
{
    public class ListRoomsQuery : PagingRequest, IRequest<PageModel<RoomModel>>
    {
        public bool OnlyAvailableRooms { get; set; }
    }
}
