using FluentValidation;
using ToursNew.Models;

namespace ToursNew.Validators;

public class TripValidator : AbstractValidator<Trip>
{
    public TripValidator()
    {
        RuleFor(t => t.Destination).NotEmpty().WithMessage("Cel wycieczki nie może być pusty");
        RuleFor(t => t.Price).NotEmpty().WithMessage("Cena jest wymagana");
        RuleFor(t => t.FromWhere).NotEmpty().WithMessage("Miejsce wylotu jest wymagane");
        RuleFor(t => t.DepartureDate).NotEmpty().WithMessage("Data wylotu jest wymagana");
        RuleFor(t => t.ReturnDate).NotEmpty().WithMessage("Data powrotu jest wymagana");
    }
}