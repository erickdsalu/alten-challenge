using Domain.Enums;
using Extensions.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Domain.Models
{
    public class Reservation
    {
        public Guid ReservationId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public static Reservation GenerateReservation(Guid customerId, Guid roomId, DateTime dateStart, DateTime dateEnd)
        {
            return new Reservation
            {
                ReservationId = Guid.NewGuid(),
                CustomerId = customerId,
                RoomId = roomId,
                StartDate = dateStart,
                EndDate = dateEnd,
                Status = ReservationStatus.Scheduled
            };
        }

        public void ValidateReservation(HotelConfiguration configuration)
        {
            var errorMessage = new StringBuilder();

            bool IsFutureStartDate()
            {
                return StartDate.Date > DateTime.UtcNow.Date;
            }

            bool IsStartDateAcceptable()
            {
                return StartDate.Date.Subtract(DateTime.UtcNow.Date).Days < configuration.MaxDaysInAdvance;
            }

            bool IsEndDateAcceptable()
            {
                return EndDate.Date >= StartDate.Date;
            }

            bool IsIntervalAcceptable()
            {
                return EndDate.Date.Subtract(StartDate.Date).Days <= configuration.MaximumReservationDays;
            }

            if (!IsFutureStartDate())
                errorMessage.AppendLine($"StartDate must be a future date");

            if (!IsStartDateAcceptable())
                errorMessage.AppendLine($"Rooms can't be reserved with more than {configuration.MaxDaysInAdvance} days in advance");

            if (!IsEndDateAcceptable())
                errorMessage.AppendLine($"EndDate must be greater or equal StartDate");

            if (!IsIntervalAcceptable())
                errorMessage.AppendLine($"There is a maximum stay of {configuration.MaximumReservationDays} days allowed per reservation");

            if (errorMessage.Length > 0)
                throw new CustomNotificationException(HttpStatusCode.BadRequest, errorMessage.ToString());
        }
    }
}
