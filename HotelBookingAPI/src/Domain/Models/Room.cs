using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Room
    {
        public Guid Id { get; private set; }
        public int Beds { get; private set; }

        public Room(Guid id, int beds)
        {
            Id = id;
            Beds = beds;
        }
    }
}
