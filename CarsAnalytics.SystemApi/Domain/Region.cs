namespace CarsAnalytics.SystemApi.Domain;

public class Region
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ICollection<Territory> Territories { get; set; } = [];
}
