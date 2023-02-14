using FluentValidation;
using UserListBackend.Models.DataModels;

namespace UserListBackend.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .Length(3, 100).WithMessage("Full name must be between 3 and 100 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .Length(2, 100).WithMessage("Country must be between 2 and 100 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .Length(2, 100).WithMessage("City must be between 2 and 100 characters.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .IsInEnum().WithMessage("Invalid gender.");
        }
    }
}
