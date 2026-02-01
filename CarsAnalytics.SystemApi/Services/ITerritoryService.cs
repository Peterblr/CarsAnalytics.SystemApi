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

    /// <summary> 
    /// Deletes a territory by its unique code. 
    /// </summary> 
    /// <param name="code">The territory code (e.g., "CA", "NY").</param> 
    /// <returns> /// An <see cref="ApiResponse{T}"/> indicating whether the deletion was successful. 
    /// Returns 204 No Content if deleted, 404 Not Found if no record exists. 
    /// </returns> 
    Task<ApiResponse<bool>> DeleteManyByCodesAsync(IEnumerable<string> codes);

    /// <summary>
    /// Updates multiple territories in the system.
    /// </summary>
    /// <param name="dtos">
    /// A collection of <see cref="TerritoryDto"/> objects containing updated values.
    /// Each territory is identified by its unique <c>Code</c>.
    /// </param>
    /// <returns>
    /// A response containing the updated territories.
    /// Territories that do not exist will not be included in the result.
    /// </returns>
    Task<ApiResponse<IEnumerable<TerritoryDto>>> UpdateManyInternalAsync(IEnumerable<TerritoryDto> dtos);
}
