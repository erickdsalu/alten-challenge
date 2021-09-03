using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models
{
    public class AvailableRoomsDaysModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Dictionary<RoomModel, IEnumerable<DateTime>> RoomsDaysAvailable { get; set; }
    }
}
