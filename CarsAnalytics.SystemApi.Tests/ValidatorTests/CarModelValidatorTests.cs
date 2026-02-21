using CarsAnalytics.SystemApi.Dto;
using CarsAnalytics.SystemApi.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace CarsAnalytics.SystemApi.Tests.ValidatorTests;

public class CarModelValidatorTests
{
    private CarModelValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new CarModelValidator();
    }

    private static readonly string[] InvalidField =
    {
        null,
        "",
        "A", // too short (min 2)
        new string('A', 201), // too long
        "BM@" // invalid char
    };

    [TestCaseSource(nameof(InvalidField))]
    public void Should_HaveError_ForInvalidMake(string make)
    {
        var model = new CarModelDto
        {
            Make = make,
            Model = "X5"
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(m => m.Make);
    }

    [TestCaseSource(nameof(InvalidField))]
    public void Should_HaveError_ForInvalidModel(string modelValue)
    {
        var model = new CarModelDto
        {
            Make = "BMW",
            Model = modelValue
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(m => m.Model);
    }

    [Test]
    public void Should_NotHaveError_ForValidFields()
    {
        var model = new CarModelDto
        {
            Make = "BMW",
            Model = "X5"
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(m => m.Model);
    }
}