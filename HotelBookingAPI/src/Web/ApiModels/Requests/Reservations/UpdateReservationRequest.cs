using FluentValidation;
using System;

namespace Web.ApiModels.Requests.Reservations
{
    public class UpdateReservationRequest
    {
        public Guid RoomId { get; set; }
        public Guid ReservationId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UpdateReservationRequestValidator : AbstractValidator<UpdateReservationRequest>
    {
        public UpdateReservationRequestValidator()
        {
            RuleFor(x => x.RoomId).NotEmpty().WithMessage("roomId is required");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("reservationId is required");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("customerId is required");
            RuleFor(x => x.StartDate).NotEmpty().Must(BeLaterThanToday).WithMessage("Must be later than today");
            RuleFor(x => x.EndDate).NotEmpty().Must(BeLaterThanToday).WithMessage("Must be later than today");
            RuleFor(x => x).Must(x => x.EndDate >= x.StartDate).WithMessage("endDate must be later or equals to startDate");
        }

        private bool BeLaterThanToday(DateTime date)
        {
            return date > DateTime.UtcNow;
        }
    }
}
