using CarsAnalytics.SystemApi.Data;
using CarsAnalytics.SystemApi.Domain;
using CarsAnalytics.SystemApi.Services;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Net;

namespace CarsAnalytics.SystemApi.Tests.ServiceTests;

public class TerritoryServiceTests
{
    private ITerritoryDataProvider _provider;
    private TerritoryService _service;

    [SetUp]
    public void SetUp()
    {
        _provider = Substitute.For<ITerritoryDataProvider>();
        _service = new TerritoryService(_provider);
    }

    [Test]
    public async Task GetTerritoriesAsync_ShouldReturnBadRequest_WhenRegionCodeIsNullOrEmpty()
    {
        // Act
        var response = await _service.GetAllAsync("");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(response.Message, Is.EqualTo("Region code is required"));
    }

    [Test]
    public async Task GetTerritoriesAsync_ShouldReturnBadRequest_WhenRegionCodeIsInvalid()
    {
        // Act
        var response = await _service.GetAllAsync("123");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(response.Message, Is.EqualTo("Region code should be two letters"));
    }

    [Test]
    public async Task GetTerritoriesAsync_ShouldReturnSuccess_WhenRegionCodeIsValid()
    {
        // Arrange
        var territories = new List<Territory>
        {
            new Territory { Code = "T1", Name = "Test Territory", RegionCode = "AB" }
        };

        _provider.GetByRegionAsync("AB").Returns(territories);

        // Act
        var response = await _service.GetAllAsync("AB");

        // Assert
        response.Data.Should().BeEquivalentTo(territories);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Message.Should().NotBeNullOrEmpty();
        response.Message.Should().Contain("successfully");
    }
}
