using CarsAnalytics.SystemApi.Dto;
using CarsAnalytics.SystemApi.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace CarsAnalytics.SystemApi.Tests.ValidatorTests;

public class TerritoryValidatorTests
{
    private TerritoryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new TerritoryValidator();
    }

    [Test]
    public void Should_HaveError_When_CodeIsNullOrEmpty()
    {
        var model = new TerritoryDto { Code = null };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Code);

        model.Code = "";
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Code);
    }

    [Test]
    public void Should_HaveError_When_CodeIsTooLong()
    {
        var model = new TerritoryDto { Code = new string('A', 11) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Code);
    }

    [Test]
    public void Should_HaveError_When_NameIsNullOrEmpty()
    {
        var model = new TerritoryDto { Name = null };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Name);

        model.Name = "";
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Name);
    }

    [Test]
    public void Should_HaveError_When_RegionCodeIsNullOrEmpty()
    {
        var model = new TerritoryDto { RegionCode = null };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.RegionCode);

        model.RegionCode = "";
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.RegionCode);
    }

    [Test]
    public void Should_HaveError_When_RegionCodeIsInvalidLength()
    {
        var model = new TerritoryDto { RegionCode = "A" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.RegionCode);

        model.RegionCode = "ABC";
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.RegionCode);
    }

    [Test]
    public void Should_NotHaveError_When_AllFieldsAreValid()
    {
        var model = new TerritoryDto
        {
            Code = "TT",
            Name = "Valid Territory",
            RegionCode = "AB"
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}