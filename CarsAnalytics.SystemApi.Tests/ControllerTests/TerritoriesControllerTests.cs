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

        _service.GetTerritoriesAsync("XX").Returns(response);

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

        _service.GetTerritoriesAsync("AB").Returns(response);

        // Act
        var result = await _controller.Get("AB") as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(result.Value, Is.EqualTo(response));
    }
}
