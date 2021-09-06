using Domain.Models;
using Persistence.Models;

namespace Persistence.Mappers
{
    public static class RoomsMapper
    {
        public static RoomPersistence AsPersistence(this Room room)
        {
            if (room is null)
                return null;
            return new RoomPersistence
            {
                Id = room.Id,
                Beds = room.Beds
            };
        }

        public static Room AsDomainModel(this RoomPersistence room)
        {
            if (room is null)
                return null;
            return new Room(id: room.Id, beds: room.Beds);
        }
    }
}
