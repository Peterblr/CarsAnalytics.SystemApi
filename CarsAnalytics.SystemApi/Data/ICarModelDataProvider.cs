using CarsAnalytics.SystemApi.Domain;

namespace CarsAnalytics.SystemApi.DataProviders.Interfaces;

public interface ICarModelDataProvider
{
    /// <summary>
    /// Retrieves all car models from the data store.
    /// </summary>
    /// <returns>A collection of <see cref="CarModel"/> objects.</returns>
    Task<IEnumerable<CarModel>> GetAllAsync();

    /// <summary>
    /// Retrieves a car model by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the car model.</param>
    /// <returns>
    /// A <see cref="CarModel"/> object if found; otherwise, <c>null</c>.
    /// </returns>
    Task<CarModel?> GetByIdAsync(int id);

    /// <summary>
    /// Creates multiple car models in the data store.
    /// </summary>
    /// <param name="models">A collection of <see cref="CarModel"/> objects to be inserted.</param>
    /// <returns>
    /// A collection of created <see cref="CarModel"/> objects, including generated identifiers.
    /// </returns>
    Task<IEnumerable<CarModel>> CreateManyAsync(IEnumerable<CarModel> models);

    /// <summary>
    /// Updates multiple car models in the data store.
    /// </summary>
    /// <param name="models">
    /// A collection of <see cref="CarModel"/> objects containing updated values.
    /// Each car model is identified by its <c>Id</c>.
    /// </param>
    /// <returns>
    /// A collection of updated <see cref="CarModel"/> objects.
    /// Models that were not found will not be included in the result.
    /// </returns>
    Task<IEnumerable<CarModel>> UpdateManyAsync(IEnumerable<CarModel> models);

    /// <summary>
    /// Deletes multiple car models by their unique identifiers.
    /// </summary>
    /// <param name="ids">A collection of car model identifiers.</param>
    /// <returns>
    /// The number of records affected (0 if no models were found with the given identifiers).
    /// </returns>
    Task<int> DeleteManyAsync(IEnumerable<int> ids);
}