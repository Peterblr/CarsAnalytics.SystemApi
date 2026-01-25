using APIResponseWrapper;
using CarsAnalytics.SystemApi.Dto;

namespace CarsAnalytics.SystemApi.Services;

public interface ITerritoryService
{
    /// <summary> 
    /// Gets territories by region code and maps them to DTOs. 
    /// </summary> 
    /// <param name="regionCode">Region identifier (US or EU).</param> 
    /// <returns>A collection of <see cref="TerritoryDto"/> objects.</returns> 
    Task<ApiResponse<IEnumerable<TerritoryDto>>> GetAllAsync(string regionCode);

    /// <summary> 
    /// Creates multiple territories in the data store and maps them to DTOs. 
    /// </summary> 
    /// <param name="dtos">A collection of <see cref="TerritoryDto"/> objects to be created.</param> 
    /// <returns> An <see cref="ApiResponse{T}"/> containing the collection of created <see cref="TerritoryDto"/> objects.</returns>
    Task<ApiResponse<IEnumerable<TerritoryDto>>> CreateManyAsync(IEnumerable<TerritoryDto> dtos);
}
