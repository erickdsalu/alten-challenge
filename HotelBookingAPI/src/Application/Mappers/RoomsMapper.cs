using Application.Models;
using Domain.Models;
using Extensions.Paging;
using System.Linq;

namespace Application.Mappers
{
    public static class RoomMapper
    {

        public static PageModel<RoomModel> AsApplicationModel(this PageModel<Room> room)
        {
            return new PageModel<RoomModel>
            {
                LastIndex = room.LastIndex,
                Items = room.Items.Select(AsApplicationModel)
            };
        }

        public static RoomModel AsApplicationModel(this Room room)
        {
            return new RoomModel
            {
                Id = room.Id,
                Beds = room.Beds
            };
        }
    }
}
