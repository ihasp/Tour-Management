using FluentValidation;
using ToursNew.Models;

namespace ToursNew.Validators
{
    public class TripValidator : AbstractValidator<Trip>    
    {
        public TripValidator() {
            RuleFor(t => t.Destination).NotEmpty().WithMessage("Cel nie może być pusty");
        }
    }
}
