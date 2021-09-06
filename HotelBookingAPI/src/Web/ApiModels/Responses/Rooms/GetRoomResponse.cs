using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ApiModels.Responses.Rooms
{
    public class GetRoomResponse
    {
        public Guid Id { get; set; }
        public int Beds { get; set; }
    }
}
