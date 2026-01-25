using System.ComponentModel;

namespace CarsAnalytics.SystemApi.Dto;

public class TerritoryDto
{
    /// <summary> 
    /// Unique code of the territory (e.g., "CA", "TX"). 
    /// </summary> 
    [DefaultValue("CA")]
    public string Code { get; set; } = string.Empty;

    /// <summary> 
    /// Description name of the territory (e.g., "California", "Texas"). 
    /// </summary> 
    [DefaultValue("California")]
    public string Name { get; set; } = string.Empty;

    /// <summary> 
    /// Region code that this territory belongs to (e.g., "US" or "EU"). 
    /// </summary> 
    [DefaultValue("US")]
    public string RegionCode { get; set; } = string.Empty;
}
