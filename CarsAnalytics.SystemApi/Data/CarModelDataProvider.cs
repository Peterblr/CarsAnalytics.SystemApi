using CarsAnalytics.SystemApi.DataProviders.Interfaces;
using CarsAnalytics.SystemApi.Domain;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CarsAnalytics.SystemApi.DataProviders;

public class CarModelDataProvider(IConfiguration configuration) : ICarModelDataProvider
{
    private const string ConName = "Default";

    private string GetConnectionString()
    {
        return configuration.GetConnectionString(ConName);
    }

    public async Task<IEnumerable<CarModel>> GetAllAsync()
    {
        const string sql = @"
            SELECT Id, Make, Model
            FROM CarModels;
        ";

        using var connection = new SqlConnection(GetConnectionString());
        return await connection.QueryAsync<CarModel>(sql);
    }

    public async Task<CarModel?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT Id, Make, Model
            FROM CarModels
            WHERE Id = @Id;
        ";

        using var connection = new SqlConnection(GetConnectionString());
        return await connection.QueryFirstOrDefaultAsync<CarModel>(sql, new { Id = id });
    }

    public async Task<IEnumerable<CarModel>> CreateManyAsync(IEnumerable<CarModel> models)
    {
        const string InsertQuery = @"
            INSERT INTO CarModels (Make, Model)
            OUTPUT INSERTED.*
            VALUES (@Make, @Model);
        ";

        using var con = new SqlConnection(GetConnectionString());
        await con.OpenAsync();

        using var transaction = await con.BeginTransactionAsync();

        var created = new List<CarModel>();

        foreach (var m in models)
        {
            var inserted = await con.QuerySingleAsync<CarModel>(InsertQuery, m, transaction);
            created.Add(inserted);
        }

        await transaction.CommitAsync();

        return created;
    }

    public async Task<IEnumerable<CarModel>> UpdateManyAsync(IEnumerable<CarModel> models)
    {
        const string UpdateQuery = @"
            UPDATE CarModels
            SET Make = @Make,
                Model = @Model
            OUTPUT INSERTED.*
            WHERE Id = @Id;
        ";

        using var con = new SqlConnection(GetConnectionString());
        await con.OpenAsync();

        using var transaction = await con.BeginTransactionAsync();

        var updated = new List<CarModel>();

        foreach (var m in models)
        {
            var result = await con.QuerySingleOrDefaultAsync<CarModel>(
                UpdateQuery,
                m,
                transaction
            );

            if (result != null)
                updated.Add(result);
        }

        await transaction.CommitAsync();

        return updated;
    }

    public async Task<int> DeleteManyAsync(IEnumerable<int> ids)
    {
        const string DeleteQuery = @"DELETE FROM CarModels WHERE Id IN @Ids;";

        using var con = new SqlConnection(GetConnectionString());
        var affected = await con.ExecuteAsync(DeleteQuery, new { Ids = ids });

        return affected;
    }
}