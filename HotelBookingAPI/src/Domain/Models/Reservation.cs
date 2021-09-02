using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public Room Room { get; set; }
        public ReservationStatus Status { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
