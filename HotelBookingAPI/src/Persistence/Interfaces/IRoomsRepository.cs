using Domain.Models;
using Extensions.Paging;
using System;
using System.Threading.Tasks;

namespace Persistence.Interfaces
{
    public interface IRoomsRepository
    {
        public Task SaveRoom(Room Room);
        public Task<PageModel<Room>> ListRooms(PagingRequest pagingRequest);
        public Task<Room> GetRoom(Guid roomId);
    }
}
