using APIResponseWrapper;
using CarsAnalytics.SystemApi.Data;
using CarsAnalytics.SystemApi.Dto;
using System.Net;

namespace CarsAnalytics.SystemApi.Services;

public class TerritoryService(ITerritoryDataProvider provider) : ITerritoryService
{
    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<TerritoryDto>>> GetTerritoriesAsync(string regionCode)
    {
        if (string.IsNullOrWhiteSpace(regionCode))
        {
            return ApiResponse<IEnumerable<TerritoryDto>>.CreateFailureResponse("Region code is required", HttpStatusCode.BadRequest);
        }

        if (!regionCode.All(char.IsLetter) || regionCode.Length != 2)
        {
            return ApiResponse<IEnumerable<TerritoryDto>>.CreateFailureResponse("Region code should be two letters", HttpStatusCode.BadRequest);
        }

        var territories = await provider.GetByRegionAsync(regionCode);

        var dtoList = territories.Select(t => new TerritoryDto 
        { 
            Code = t.Code, 
            Name = t.Name, 
            RegionCode = t.RegionCode 
        });

        return ApiResponse<IEnumerable<TerritoryDto>>.CreateSuccessResponse(dtoList);
    }
}
