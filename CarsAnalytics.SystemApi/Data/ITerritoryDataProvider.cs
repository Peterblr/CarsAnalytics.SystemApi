
using CarsAnalytics.SystemApi.Domain;

namespace CarsAnalytics.SystemApi.Data;

public interface ITerritoryDataProvider
{
    /// <summary> 
    /// Retrieves a list of territories by region code (e.g., "US" or "EU"). 
    /// </summary> 
    /// <param name="regionCode">Region identifier (US or EU).</param> 
    /// <returns>A collection of <see cref="Territory"/> objects.</returns>
    Task<IEnumerable<Territory>> GetByRegionAsync(string regionCode);

    /// <summary> 
    /// Creates multiple territories in the data store. 
    /// </summary> 
    /// <param name="territories">A collection of <see cref="Territory"/> objects to be inserted.</param> 
    /// <returns> A collection of <see cref="Territory"/> objects that were successfully created. </returns>
    Task<IEnumerable<Territory>> CreateManyAsync(IEnumerable<Territory> territories);

    /// <summary> 
    /// Deletes territories by their unique code. 
    /// </summary> 
    /// <param name="code">The territory code (e.g., "CA", "NY").</param> 
    /// <returns>The number of records affected (0 if no territory was found with the given code).</returns>
    Task<int> DeleteManyByCodesAsync(IEnumerable<string> codes);

    /// <summary>
    /// Updates multiple territories in the data store.
    /// </summary>
    /// <param name="territories">
    /// A collection of <see cref="Territory"/> objects containing updated values.
    /// Each territory is identified by its unique <c>Code</c>.
    /// </param>
    /// <returns>
    /// A collection of updated <see cref="Territory"/> objects.  
    /// Territories that were not found will not be included in the result.
    /// </returns>
    Task<IEnumerable<Territory>> UpdateManyInternalAsync(IEnumerable<Territory> territories);
}
