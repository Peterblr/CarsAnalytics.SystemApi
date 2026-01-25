using FluentValidation;
using CarsAnalytics.SystemApi.Dto;
using CarsAnalytics.SystemApi.Helpers;

namespace CarsAnalytics.SystemApi.Validators;

public class TerritoryValidator : AbstractValidator<TerritoryDto>
{
    public TerritoryValidator()
    {
        RuleFor(t => t.Code)
            .NotEmpty().WithMessage("Code is required")
            .Length(DatabaseConstraints.DbDConstraint2Char, DatabaseConstraints.DbDConstraint10Char)
            .WithMessage($"Code must be between {DatabaseConstraints.DbDConstraint2Char} and {DatabaseConstraints.DbDConstraint10Char} characters")
            .Matches(@"^[A-Za-z]+$").WithMessage("RegionCode must contain only letters");

        RuleFor(t => t.Name)
            .NotEmpty().WithMessage("Name is required")
            .Matches(@"^[A-Za-z\s]+$").WithMessage("RegionCode must contain only letters");

        RuleFor(t => t.RegionCode)
            .NotEmpty().WithMessage("RegionCode is required")
            .Length(DatabaseConstraints.DbDConstraint2Char, DatabaseConstraints.DbDConstraint2Char)
            .WithMessage($"RegionCode must be exactly {DatabaseConstraints.DbDConstraint2Char} letters")
            .Matches(@"^[A-Za-z]+$").WithMessage("RegionCode must contain only letters");
    }
}
