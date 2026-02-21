using CarsAnalytics.SystemApi.Dto;
using CarsAnalytics.SystemApi.Helpers;
using FluentValidation;

namespace CarsAnalytics.SystemApi.Validators;

public class CarModelValidator : AbstractValidator<CarModelDto>
{
    public CarModelValidator()
    {
        RuleFor(m => m.Make)
            .NotEmpty().WithMessage("Make is required")
            .Length(DatabaseConstraints.DbDConstraint2Char, DatabaseConstraints.DbDConstraint50Char)
            .WithMessage($"Make must be between {DatabaseConstraints.DbDConstraint2Char} and {DatabaseConstraints.DbDConstraint50Char} characters")
            .Matches(@"^[A-Za-z0-9\s\-]+$").WithMessage("Make must contain only letters, digits, spaces or hyphens");

        RuleFor(m => m.Model)
            .NotEmpty().WithMessage("Model is required")
            .Length(DatabaseConstraints.DbDConstraint2Char, DatabaseConstraints.DbDConstraint50Char)
            .WithMessage($"Model must be between {DatabaseConstraints.DbDConstraint2Char} and {DatabaseConstraints.DbDConstraint50Char} characters")
            .Matches(@"^[A-Za-z0-9\s\-]+$").WithMessage("Model must contain only letters, digits, spaces or hyphens");
    }
}
