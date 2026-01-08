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
    Task<ApiResponse<IEnumerable<TerritoryDto>>> GetTerritoriesAsync(string regionCode);
}
