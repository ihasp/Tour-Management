using FluentValidation;
using ToursNew.Models;

namespace ToursNew.Validators
{
    public class ReservationValidator : AbstractValidator<Reservation>
    { 
        public ReservationValidator()
        {
            RuleFor(r => r.IDClient).NotEmpty().WithMessage("Klucz obcy ID Klienta nie może być puste");
            RuleFor(r => r.IDTrip).NotEmpty().WithMessage("Klucz obcy ID Wycieczki nie może być puste");
        }
    }
    
}
