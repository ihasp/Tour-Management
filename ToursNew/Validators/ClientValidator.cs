using FluentValidation;
using ToursNew.Models;

namespace ToursNew.Validators;

public class ClientValidator : AbstractValidator<Client>
{
    public ClientValidator()
    {
        RuleFor(c => c.LastName).NotEmpty().WithMessage("Nazwisko nie może być puste");
        RuleFor(c => c.Phone).MaximumLength(13).WithMessage("Numer telefonu może mieć max 13 znaków")
            .MinimumLength(9).WithMessage("Numer telefonu musi mieć min 9 cyfr");
        RuleFor(c => c.Name).MaximumLength(30).WithMessage("Imię nie może być dłuższe niż 30 znaków");
        RuleFor(c => c.Email).MaximumLength(100).WithMessage("Email nie może przekraczać 100 znaków.");
    }
}