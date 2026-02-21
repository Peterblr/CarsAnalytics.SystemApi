using APIResponseWrapper;
using CarsAnalytics.SystemApi.DataProviders.Interfaces;
using CarsAnalytics.SystemApi.Dto;
using CarsAnalytics.SystemApi.Services.Interfaces;

namespace CarsAnalytics.SystemApi.Services;

public class CarModelService(ICarModelDataProvider provider) : ICarModelService
{
    public async Task<ApiResponse<IEnumerable<CarModelDto>>> GetAllAsync()
    {
        var items = await provider.GetAllAsync();

        var dtoList = items.Select(m => new CarModelDto
        {
            Id = m.Id,
            Make = m.Make,
            Model = m.Model
        });

        return ApiResponse<IEnumerable<CarModelDto>>.CreateSuccessResponse(dtoList);
    }
    public Task<IEnumerable<CarModelDto>> CreateManyAsync(IEnumerable<CarModelDto> models)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteManyAsync(IEnumerable<int> ids)
    {
        throw new NotImplementedException();
    }

    public Task<CarModelDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CarModelDto>> UpdateManyAsync(IEnumerable<CarModelDto> models)
    {
        throw new NotImplementedException();
    }
}