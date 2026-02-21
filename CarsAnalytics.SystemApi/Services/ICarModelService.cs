using APIResponseWrapper;
using CarsAnalytics.SystemApi.Dto;

namespace CarsAnalytics.SystemApi.Services.Interfaces;

public interface ICarModelService
{
    /// <summary>
    /// Retrieves all car models.
    /// </summary>
    /// <returns>A collection of <see cref="CarModelDto"/> objects.</returns>
    Task<ApiResponse<IEnumerable<CarModelDto>>> GetAllAsync();

    /// <summary>
    /// Retrieves a car model by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the car model.</param>
    /// <returns>
    /// A <see cref="CarModelDto"/> object if found; otherwise, <c>null</c>.
    /// </returns>
    Task<CarModelDto?> GetByIdAsync(int id);

    /// <summary>
    /// Creates multiple car models in the data store.
    /// </summary>
    /// <param name="models">A collection of <see cref="CarModelDto"/> objects to be inserted.</param>
    /// <returns>
    /// A collection of created <see cref="CarModelDto"/> objects, including generated identifiers.
    /// </returns>
    Task<IEnumerable<CarModelDto>> CreateManyAsync(IEnumerable<CarModelDto> models);

    /// <summary>
    /// Updates multiple car models in the data store.
    /// </summary>
    /// <param name="models">
    /// A collection of <see cref="CarModelDto"/> objects containing updated values.
    /// Each car model is identified by its <c>Id</c>.
    /// </param>
    /// <returns>
    /// A collection of updated <see cref="CarModelDto"/> objects.
    /// Models that were not found will not be included in the result.
    /// </returns>
    Task<IEnumerable<CarModelDto>> UpdateManyAsync(IEnumerable<CarModelDto> models);

    /// <summary>
    /// Deletes multiple car models by their unique identifiers.
    /// </summary>
    /// <param name="ids">A collection of car model identifiers.</param>
    /// <returns>
    /// The number of records affected (0 if no models were found with the given identifiers).
    /// </returns>
    Task<int> DeleteManyAsync(IEnumerable<int> ids);
}