using CarsAnalytics.SystemApi.Domain;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CarsAnalytics.SystemApi.Data;

public class TerritoryDataProvider(IConfiguration configuration) : ITerritoryDataProvider
{
    private const string ConName = "Default";

    private string GetConnectionString() => configuration.GetConnectionString(ConName);

    /// <inheritdoc />
    public async Task<IEnumerable<Territory>> GetByRegionAsync(string regionCode)
    {
        const string GetAllQuery = @"
            SELECT * FROM Territories
                WHERE RegionCode=@RegionCode;
            ";

        using var con = new SqlConnection(GetConnectionString());
        return await con.QueryAsync<Territory>(GetAllQuery, new { RegionCode = regionCode });
    }
}
