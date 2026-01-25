using CarsAnalytics.SystemApi.Domain;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CarsAnalytics.SystemApi.Data;

public class TerritoryDataProvider(IConfiguration configuration) : ITerritoryDataProvider
{
    private const string ConName = "Default";

    private string GetConnectionString()
    {
        return configuration.GetConnectionString(ConName);
    }

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

    public async Task<IEnumerable<Territory>> CreateManyAsync(IEnumerable<Territory> territories)
    {
        const string InsertQuery = @"
            INSERT INTO Territories (Code, Name, RegionCode)
            OUTPUT INSERTED.*
            VALUES (@Code, @Name, @RegionCode);
        ";

        using var con = new SqlConnection(GetConnectionString());
        await con.OpenAsync();

        using var transaction = await con.BeginTransactionAsync();

        var created = new List<Territory>();

        foreach (var t in territories)
        {
            var inserted = await con.QuerySingleAsync<Territory>(InsertQuery, t, transaction);
            created.Add(inserted);
        }

        await transaction.CommitAsync();

        return created;
    }

    public async Task<int> DeleteManyByCodesAsync(IEnumerable<string> codes)
    {
        const string DeleteQuery = @" DELETE FROM Territories WHERE Code IN @Codes;";
        using var con = new SqlConnection(GetConnectionString());
        var affected = await con.ExecuteAsync(DeleteQuery, new { codes });
        return affected;
    }
}
