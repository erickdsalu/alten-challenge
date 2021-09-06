using FluentValidation;
using System;

namespace Web.ApiModels.Requests.Reservations
{
    public class CreateReservationRequest
    {
        public Guid CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
    {
        public CreateReservationRequestValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Please specify a first name");
            RuleFor(x => x.RoomId).NotEmpty().WithMessage("Must be later than today");
            RuleFor(x => x.StartDate).NotEmpty().Must(BeLaterThanToday).WithMessage("Must be later than today");
            RuleFor(x => x.EndDate).NotEmpty().Must(BeLaterThanToday).WithMessage("Must be later than today");
        }

        private bool BeLaterThanToday(DateTime date)
        {
            return date > DateTime.UtcNow;
        }
    }
}
