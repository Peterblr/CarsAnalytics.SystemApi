using System.Net;
using APIResponseWrapper;
using CarsAnalytics.SystemApi.Controllers;
using CarsAnalytics.SystemApi.Dto;
using CarsAnalytics.SystemApi.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace CarsAnalytics.SystemApi.Tests.ControllerTests;

public class TerritoriesControllerTests
{
    private ITerritoryService _service;
    private TerritoriesController _controller;

    [SetUp]
    public void SetUp()
    {
        _service = Substitute.For<ITerritoryService>();
        _controller = new TerritoriesController(_service);
    }

    [Test]
    public async Task Get_ShouldReturnBadRequest_WhenServiceReturnsError()
    {
        // Arrange
        var response = ApiResponse<IEnumerable<TerritoryDto>>
            .CreateFailureResponse("Region code is required", HttpStatusCode.BadRequest);

        _service.GetAllAsync("XX").Returns(response);

        // Act
        var result = await _controller.Get("XX") as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        Assert.That(result.Value, Is.EqualTo(response));
    }

    [Test]
    public async Task Get_ShouldReturnOk_WhenServiceReturnsSuccess()
    {
        // Arrange
        var territories = new List<TerritoryDto>
        {
            new TerritoryDto { Code = "T1", Name = "Test Territory", RegionCode = "AB" }
        };

        var response = ApiResponse<IEnumerable<TerritoryDto>>
            .CreateSuccessResponse(territories);

        _service.GetAllAsync("AB").Returns(response);

        // Act
        var result = await _controller.Get("AB") as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(result.Value, Is.EqualTo(response));
    }

    [Test]
    public async Task PostMany_ShouldReturnBadRequest_WhenInputIsInvalid()
    {
        // Arrange
        var invalidDtos = new List<TerritoryDto> { null };
        _service.CreateManyAsync(invalidDtos).Returns(
            ApiResponse<IEnumerable<TerritoryDto>>.CreateFailureResponse("At least one territory is required", HttpStatusCode.BadRequest));

        // Act
        var result = await _controller.PostMany(invalidDtos) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        Assert.That(result.Value, Is.Not.Null);
        var response = (ApiResponse<IEnumerable<TerritoryDto>>)result.Value;
        Assert.That(response.Message, Is.EqualTo("At least one territory is required"));
    }

    [Test]
    public async Task PostMany_ShouldReturnCreated_WhenInputIsValid()
    {
        // Arrange
        var dtos = new List<TerritoryDto>
        {
            new TerritoryDto { Code = "TB", Name = "Territory1", RegionCode = "AB" }
        };
        var successResponse = ApiResponse<IEnumerable<TerritoryDto>>.CreateSuccessResponse(dtos, null, HttpStatusCode.Created);
        successResponse.Message = "Territories created successfully (1 records)";
        _service.CreateManyAsync(dtos).Returns(successResponse);

        // Act
        var result = await _controller.PostMany(dtos) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
        Assert.That(result.Value, Is.Not.Null);
        var response = (ApiResponse<IEnumerable<TerritoryDto>>)result.Value;
        Assert.That(response.Message, Does.Contain("created successfully"));
        Assert.That(response.Data, Is.EquivalentTo(dtos));
    }

    [Test]
    public async Task DeleteMany_ShouldReturnNotFound_WhenNoTerritoriesFound()
    {
        // Arrange
        var codes = new List<string> { "AB", "BC" }; 
        _service.DeleteManyByCodesAsync(codes).Returns(
            ApiResponse<bool>.CreateFailureResponse("No territories found for provided codes", HttpStatusCode.NotFound));

        // Act
        var result = await _controller.DeleteMany(codes) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
        var response = (ApiResponse<bool>)result.Value;
        Assert.That(response.Message, Is.EqualTo("No territories found for provided codes"));
    }

    [Test]
    public async Task DeleteMany_ShouldReturnNoContent_WhenDeletionSucceeds()
    {
        // Arrange
        var codes = new List<string> { "TA", "TB" };
        var successResponse = ApiResponse<bool>.CreateSuccessResponse(true, null, HttpStatusCode.NoContent);
        successResponse.Message = "Deleted 2 territories successfully";
        _service.DeleteManyByCodesAsync(codes).Returns(successResponse);

        // Act
        var result = await _controller.DeleteMany(codes) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
        var response = (ApiResponse<bool>)result.Value;
        Assert.That(response.Data, Is.True);
        Assert.That(response.Message, Does.Contain("Deleted 2 territories successfully"));
    }

    [Test]
    public async Task UpdateMany_ShouldReturnBadRequest_WhenInputIsInvalid()
    {
        // Arrange
        var invalidDtos = new List<TerritoryDto> { null };

        var failureResponse = ApiResponse<IEnumerable<TerritoryDto>>
            .CreateFailureResponse("At least one territory is required", HttpStatusCode.BadRequest);

        _service.UpdateManyInternalAsync(invalidDtos).Returns(failureResponse);

        // Act
        var result = await _controller.UpdateMany(invalidDtos) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));

        var response = (ApiResponse<IEnumerable<TerritoryDto>>)result.Value;
        Assert.That(response.Message, Is.EqualTo("At least one territory is required"));
    }

    [Test]
    public async Task UpdateMany_ShouldReturnNotFound_WhenSomeTerritoriesDoNotExist()
    {
        // Arrange
        var dtos = new List<TerritoryDto>
        {
            new TerritoryDto { Code = "AA", Name = "Name", RegionCode = "US" }
        };

        var failureResponse = ApiResponse<IEnumerable<TerritoryDto>>
            .CreateFailureResponse("Some territories do not exist: AA", HttpStatusCode.NotFound);

        _service.UpdateManyInternalAsync(dtos).Returns(failureResponse);

        // Act
        var result = await _controller.UpdateMany(dtos) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));

        var response = (ApiResponse<IEnumerable<TerritoryDto>>)result.Value;
        Assert.That(response.Message, Does.Contain("Some territories do not exist"));
    }

    [Test]
    public async Task UpdateMany_ShouldReturnInternalServerError_WhenUpdateFails()
    {
        // Arrange
        var dtos = new List<TerritoryDto>
        {
            new TerritoryDto { Code = "AA", Name = "Name", RegionCode = "US" }
        };

        var failureResponse = ApiResponse<IEnumerable<TerritoryDto>>
            .CreateFailureResponse("Failed to update territories", HttpStatusCode.InternalServerError);

        _service.UpdateManyInternalAsync(dtos).Returns(failureResponse);

        // Act
        var result = await _controller.UpdateMany(dtos) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var response = (ApiResponse<IEnumerable<TerritoryDto>>)result.Value;
        Assert.That(response.Message, Is.EqualTo("Failed to update territories"));
    }

    [Test]
    public async Task UpdateMany_ShouldReturnOk_WhenUpdateSucceeds()
    {
        // Arrange
        var dtos = new List<TerritoryDto>
        {
            new TerritoryDto { Code = "AA", Name = "NewName", RegionCode = "US" }
        };

        var successResponse = ApiResponse<IEnumerable<TerritoryDto>>
            .CreateSuccessResponse(dtos, null, HttpStatusCode.OK);

        successResponse.Message = "Territories updated successfully (1 records)";

        _service.UpdateManyInternalAsync(dtos).Returns(successResponse);

        // Act
        var result = await _controller.UpdateMany(dtos) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        var response = (ApiResponse<IEnumerable<TerritoryDto>>)result.Value;
        Assert.That(response.Data, Is.EquivalentTo(dtos));
        Assert.That(response.Message, Does.Contain("updated successfully"));
    }
}
