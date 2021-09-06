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
        public Guid ReservationId { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid RoomId { get; private set; }
        public ReservationStatus Status { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsActive { get; private set; }

        public Reservation(Guid reservationId, Guid customerId, Guid roomId, ReservationStatus status, DateTime startDate, DateTime endDate, bool isActive)
        {
            ReservationId = reservationId;
            CustomerId = customerId;
            RoomId = roomId;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
        }

        public static Reservation GenerateReservation(Guid customerId, Guid roomId, DateTime startDate, DateTime endDate)
        {
            return new Reservation
            (
                Guid.NewGuid(),
                customerId,
                roomId,
                ReservationStatus.Scheduled,
                startDate,
                endDate,
                true
            );
        }

        public void ValidateReservation(Configuration configuration)
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

        public void UpdateReservation(Configuration configuration, DateTime? startDate, DateTime? endDate)
        {
            StartDate = startDate ?? StartDate;
            EndDate = endDate ?? EndDate;

            ValidateReservation(configuration);
        }

        public void CancelReservation()
        {
            IsActive = false;
        }
    }
}
