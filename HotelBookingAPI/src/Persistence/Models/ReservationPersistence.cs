using Domain.Enums;
using System;

namespace Persistence.Models
{
    public class ReservationPersistence
    {
        public Guid ReservationId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
