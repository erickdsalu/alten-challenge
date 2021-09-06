using Application.Models;
using Extensions.Paging;
using System.Linq;
using Web.ApiModels.Responses.Rooms;

namespace Web.Mappers
{
    public static class RoomsMapper
    {
        public static PageModel<GetRoomResponse> AsResponseModel(this PageModel<RoomModel> room)
        {
            return new PageModel<GetRoomResponse>
            {
                LastIndex = room.LastIndex,
                Items = room.Items.Select(AsResponseModel)
            };
        }

        public static GetRoomResponse AsResponseModel(this RoomModel roomModel)
        {
            return new GetRoomResponse
            {
                Id = roomModel.Id,
                Beds = roomModel.Beds
            };
        }
    }
}
