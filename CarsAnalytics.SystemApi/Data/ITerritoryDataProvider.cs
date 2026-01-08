
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
}
