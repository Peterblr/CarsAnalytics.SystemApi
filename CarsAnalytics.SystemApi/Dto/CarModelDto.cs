using System.ComponentModel;

namespace CarsAnalytics.SystemApi.Dto;

public class CarModelDto
{
    // <summary>
    /// Unique identifier of the car model.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Manufacturer of the car (e.g., "BMW", "Toyota").
    /// </summary>
    [DefaultValue("BMW")]
    public string Make { get; set; } = string.Empty;

    /// <summary>
    /// Specific model name (e.g., "X5", "Camry").
    /// </summary>
    [DefaultValue("X5")]
    public string Model { get; set; } = string.Empty;
}
