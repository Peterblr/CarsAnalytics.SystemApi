using APIResponseWrapper;
using CarsAnalytics.SystemApi.Data;
using CarsAnalytics.SystemApi.Domain;
using CarsAnalytics.SystemApi.Dto;
using CarsAnalytics.SystemApi.Validators;
using System.Net;

namespace CarsAnalytics.SystemApi.Services;

public class TerritoryService(ITerritoryDataProvider provider) : ITerritoryService
{
    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<TerritoryDto>>> GetAllAsync(string regionCode)
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

    public async Task<ApiResponse<IEnumerable<TerritoryDto>>> CreateManyAsync(IEnumerable<TerritoryDto> dtos)
    {
        var response = ApiResponse<IEnumerable<TerritoryDto>>.CreateFailureResponse(string.Empty);

        if (!await ValidateInput(dtos, response))
            return response;

        if (!await ValidateNoDuplicateInDatabaseByRegionCode(dtos, response))
            return response;

        var entities = dtos.Select(dto => new Territory
        {
            Code = dto.Code,
            Name = dto.Name,
            RegionCode = dto.RegionCode
        });

        var created = await provider.CreateManyAsync(entities);

        if (!created.Any())
        {
            return ApiResponse<IEnumerable<TerritoryDto>>.CreateFailureResponse(
                "Failed while saving territories",
                HttpStatusCode.InternalServerError
            );
        }

        var result = created.Select(t => new TerritoryDto
        {
            Code = t.Code,
            Name = t.Name,
            RegionCode = t.RegionCode
        });

        var successResponse = ApiResponse<IEnumerable<TerritoryDto>>.CreateSuccessResponse(result, null, HttpStatusCode.Created);
        successResponse.Message = $"Territories created successfully ({result.Count()} records)";
        return successResponse;
    }

    private async Task<bool> ValidateInput(IEnumerable<TerritoryDto> dtos, ApiResponse<IEnumerable<TerritoryDto>> response)
    {
        if (dtos.Any(x => x is null))
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = "At least one territory is required";
            return false;
        }

        var validator = new TerritoryValidator();
        var validationTasks = dtos.Select(dto => validator.ValidateAsync(dto));
        var validationResults = await Task.WhenAll(validationTasks);

        if (validationResults.Any(r => !r.IsValid))
        {
            var errors = validationResults
                .SelectMany(r => r.Errors)
                .Select(e => e.ErrorMessage)
                .Distinct()
                .ToList();

            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = string.Join("; ", errors);
            return false;
        }

        var duplicateDtos = dtos
            .GroupBy(d => new { d.Code, d.Name, d.RegionCode })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateDtos.Any())
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = "Request contains duplicate territories: " +
                string.Join("; ", duplicateDtos.Select(d => $"Code={d.Code}, Name={d.Name}, Region={d.RegionCode}"));
            return false;
        }

        return true;
    }

    private async Task<bool> ValidateNoDuplicateInDatabaseByRegionCode(IEnumerable<TerritoryDto> dtos, ApiResponse<IEnumerable<TerritoryDto>> response)
    {
        var regionCode = dtos.First().RegionCode;
        var existing = await provider.GetByRegionAsync(regionCode);

        var normalizedExisting = existing.Select(e => new
        {
            Code = e.Code?.Trim(),
            Name = e.Name?.Trim(),
            RegionCode = e.RegionCode?.Trim()
        }).ToList();

        var normalizedDtos = dtos.Select(d => new
        {
            Code = d.Code?.Trim(),
            Name = d.Name?.Trim(),
            RegionCode = d.RegionCode?.Trim()
        }).ToList();

        var codeDuplicates = normalizedDtos
            .Where(d => normalizedExisting.Any(e =>
                string.Equals(e.RegionCode, d.RegionCode, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(e.Code, d.Code, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        var nameDuplicates = normalizedDtos
            .Where(d => normalizedExisting.Any(e =>
                string.Equals(e.RegionCode, d.RegionCode, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(e.Name, d.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (codeDuplicates.Any() || nameDuplicates.Any())
        {
            var errors = new List<string>();

            if (codeDuplicates.Any())
                errors.Add("Duplicate Code in region: " +
                    string.Join("; ", codeDuplicates.Select(d => $"Code={d.Code}, Region={d.RegionCode}")));

            if (nameDuplicates.Any())
                errors.Add("Duplicate Name in region: " +
                    string.Join("; ", nameDuplicates.Select(d => $"Name={d.Name}, Region={d.RegionCode}")));

            response.StatusCode = HttpStatusCode.Conflict;
            response.Message = string.Join(" | ", errors);
            return false;
        }

        return true;
    }
}
