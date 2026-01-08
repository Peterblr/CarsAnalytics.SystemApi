namespace CarsAnalytics.SystemApi.Domain;

public class CarModel
{
    public int Id { get; set; }
    public string Make { get; set; } = string.Empty; 
    public string Model { get; set; } = string.Empty; 
    public ICollection<TerritoryCarStat> Stats { get; set; } = [];
}
