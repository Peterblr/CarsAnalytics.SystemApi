namespace CarsAnalytics.SystemApi.Dto;

public class TerritoryDto
{
    /// <summary> 
    /// Unique code of the territory (e.g., "CA", "TX"). 
    /// </summary> 
    /// <example>CA</example>
    public string Code { get; set; } = string.Empty;

    /// <summary> 
    /// Description name of the territory (e.g., "California", "Texas"). 
    /// </summary> 
    /// <example>California</example>
    public string Name { get; set; } = string.Empty;

    /// <summary> 
    /// Region code that this territory belongs to (e.g., "US" or "EU"). 
    /// </summary> 
    /// <example>US</example>
    public string RegionCode { get; set; } = string.Empty;
}
