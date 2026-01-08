namespace CarsAnalytics.SystemApi.Domain;

public class TerritoryCarStat
{
    public int Id { get; set; }
    public string TerritoryCode { get; set; } = string.Empty; 
    public int CarModelId { get; set; }
    public int Sales { get; set; }
    public Territory? Territory { get; set; }
    public CarModel? CarModel { get; set; }
}
