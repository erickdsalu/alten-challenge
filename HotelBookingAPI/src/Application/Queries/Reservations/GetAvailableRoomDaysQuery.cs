using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries.Rooms
{
    public class GetAvailableRoomDaysQuery
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
