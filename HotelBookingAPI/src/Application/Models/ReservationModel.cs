using Application.Enums;
using System;

namespace Application.Models
{
    public class ReservationModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public ReservationStatusModel Status { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
