
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
}
