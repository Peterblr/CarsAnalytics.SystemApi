using CarsAnalytics.SystemApi.Data;
using CarsAnalytics.SystemApi.Domain;
using CarsAnalytics.SystemApi.Dto;
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
            new Territory { Code = "TB", Name = "Test Territory", RegionCode = "AB" }
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

    [Test]
    public async Task CreateManyAsync_ShouldReturnBadRequest_WhenInputIsInvalid()
    {
        // Arrange
        var invalidDtos = new List<TerritoryDto> { null };

        // Act
        var response = await _service.CreateManyAsync(invalidDtos);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(response.Message, Is.EqualTo("At least one territory is required"));
    }

    [Test]
    public async Task CreateManyAsync_ShouldReturnConflict_WhenDuplicatesExistInDatabase()
    {
        // Arrange
        var dtos = new List<TerritoryDto>
        {
            new TerritoryDto { Code = "AC", Name = "TerritoryAC", RegionCode = "AB" }
        };

        var existingTerritories = new List<Territory>
        {
            new Territory { Code = "AC", Name = "TerritoryAC", RegionCode = "AB" }
        };

        _provider.GetByRegionAsync("AB").Returns(existingTerritories);

        // Act
        var response = await _service.CreateManyAsync(dtos);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        Assert.That(response.Message, Does.Contain("Duplicate Code in region"));
    }

    [Test]
    public async Task CreateManyAsync_ShouldReturnCreated_WhenInputIsValid()
    {
        // Arrange
        var dtos = new List<TerritoryDto>
        {
            new TerritoryDto { Code = "TB", Name = "Territory", RegionCode = "AB" }
        };

        var createdTerritories = new List<Territory>
        {
            new Territory { Code = "TB", Name = "Territory", RegionCode = "AB" }
        };

        _provider.GetByRegionAsync("AB").Returns(new List<Territory>());
        _provider.CreateManyAsync(Arg.Any<IEnumerable<Territory>>()).Returns(createdTerritories);

        // Act
        var response = await _service.CreateManyAsync(dtos);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(response.Data, Is.Not.Empty);
        Assert.That(response.Message, Does.Contain("created successfully"));
    }

    [Test]
    public async Task DeleteManyByCodesAsync_ShouldReturnBadRequest_WhenCodesAreEmpty()
    {
        // Act
        var response = await _service.DeleteManyByCodesAsync(new List<string>());

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(response.Message, Is.EqualTo("At least one code is required"));
    }

    [Test]
    public async Task DeleteManyByCodesAsync_ShouldReturnNotFound_WhenNoTerritoriesDeleted()
    {
        // Arrange
        _provider.DeleteManyByCodesAsync(Arg.Any<IEnumerable<string>>()).Returns(0);

        // Act
        var response = await _service.DeleteManyByCodesAsync(new List<string> { "TB" });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(response.Message, Does.Contain("No territories found"));
    }

    [Test]
    public async Task DeleteManyByCodesAsync_ShouldReturnNoContent_WhenTerritoriesDeleted()
    {
        // Arrange
        _provider.DeleteManyByCodesAsync(Arg.Any<IEnumerable<string>>()).Returns(2);

        // Act
        var response = await _service.DeleteManyByCodesAsync(new List<string> { "TA", "TB" });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(response.Data, Is.True);
        Assert.That(response.Message, Does.Contain("Deleted 2 territories successfully"));
    }

    [Test]
    public async Task UpdateManyInternalAsync_ShouldReturnBadRequest_WhenInputIsInvalid()
    {
        // Arrange
        var invalidDtos = new List<TerritoryDto> { null };

        // Act
        var response = await _service.UpdateManyInternalAsync(invalidDtos);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Message.Should().Be("At least one territory is required");
    }

    [Test]
    public async Task UpdateManyInternalAsync_ShouldReturnNotFound_WhenTerritoriesDoNotExist()
    {
        // Arrange
        var dtos = new List<TerritoryDto>
        {
            new TerritoryDto { Code = "AA", Name = "Name", RegionCode = "US" }
        };

        _provider.GetByRegionAsync("US").Returns(new List<Territory>());

        // Act
        var response = await _service.UpdateManyInternalAsync(dtos);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Message.Should().Contain("Some territories do not exist");
    }

    [Test]
    public async Task UpdateManyInternalAsync_ShouldReturnInternalServerError_WhenProviderReturnsEmptyList()
    {
        // Arrange
        var dtos = new List<TerritoryDto>
        {
            new TerritoryDto { Code = "AA", Name = "Name", RegionCode = "US" }
        };

        _provider.GetByRegionAsync("US").Returns(new List<Territory>
        {
            new Territory { Code = "AA", Name = "Old", RegionCode = "US" }
        });

        _provider.UpdateManyInternalAsync(Arg.Any<IEnumerable<Territory>>())
                 .Returns(new List<Territory>()); 

        // Act
        var response = await _service.UpdateManyInternalAsync(dtos);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Message.Should().Be("Failed to update territories");
    }

    [Test]
    public async Task UpdateManyInternalAsync_ShouldReturnSuccess_WhenUpdateIsSuccessful()
    {
        // Arrange
        var dtos = new List<TerritoryDto>
        {
            new TerritoryDto { Code = "AA", Name = "NewName", RegionCode = "US" }
        };

        var existing = new List<Territory>
        {
            new Territory { Code = "AA", Name = "OldName", RegionCode = "US" }
        };

        var updated = new List<Territory>
        {
            new Territory { Code = "AA", Name = "NewName", RegionCode = "US" }
        };

        _provider.GetByRegionAsync("US").Returns(existing);
        _provider.UpdateManyInternalAsync(Arg.Any<IEnumerable<Territory>>()).Returns(updated);

        // Act
        var response = await _service.UpdateManyInternalAsync(dtos);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().BeEquivalentTo(updated);
        response.Message.Should().Contain("updated successfully");
    }

    [Test]
    public async Task UpdateManyInternalAsync_ShouldMapDtosToEntitiesCorrectly()
    {
        // Arrange
        var dtos = new List<TerritoryDto>
        {
            new TerritoryDto { Code = "AA", Name = "NewName", RegionCode = "US" }
        };

        _provider.GetByRegionAsync("US").Returns(new List<Territory>
        {
            new Territory { Code = "AA", Name = "OldName", RegionCode = "US" }
        });

        IEnumerable<Territory> capturedEntities = null;

        _provider.UpdateManyInternalAsync(Arg.Do<IEnumerable<Territory>>(x => capturedEntities = x))
                 .Returns(new List<Territory>
                     {
                     new Territory { Code = "AA", Name = "NewName", RegionCode = "US" }
                     });

        // Act
        await _service.UpdateManyInternalAsync(dtos);

        // Assert
        capturedEntities.Should().NotBeNull();
        capturedEntities.Should().HaveCount(1);

        var entity = capturedEntities.First();
        entity.Code.Should().Be("AA");
        entity.Name.Should().Be("NewName");
        entity.RegionCode.Should().Be("US");
    }
}
