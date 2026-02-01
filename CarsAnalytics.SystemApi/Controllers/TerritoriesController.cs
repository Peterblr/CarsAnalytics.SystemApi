using APIResponseWrapper;
using CarsAnalytics.SystemApi.Dto;
using CarsAnalytics.SystemApi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarsAnalytics.SystemApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TerritoriesController(ITerritoryService territoryService) : ControllerBase
{
    [HttpGet("{regionCode}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns list of territories", typeof(ApiResponse<IEnumerable<TerritoryDto>>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Region code is required or invalid")]
    public async Task<IActionResult> Get(string regionCode)
    {
        var apiResponse = await territoryService.GetAllAsync(regionCode);
        return StatusCode((int)apiResponse.StatusCode, apiResponse);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "Territories created", typeof(ApiResponse<IEnumerable<TerritoryDto>>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Duplicate territories found", typeof(ApiResponse<IEnumerable<TerritoryDto>>))]
    public async Task<IActionResult> PostMany([FromBody] IEnumerable<TerritoryDto> dtos)
    {
        var apiResponse = await territoryService.CreateManyAsync(dtos);
        return StatusCode((int)apiResponse.StatusCode, apiResponse);
    }

    [HttpPut]
    [SwaggerResponse(StatusCodes.Status200OK, "Territories updated", typeof(ApiResponse<IEnumerable<TerritoryDto>>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Some territories do not exist")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Failed to update territories")]
    public async Task<IActionResult> UpdateMany([FromBody] IEnumerable<TerritoryDto> dtos)
    {
        var apiResponse = await territoryService.UpdateManyInternalAsync(dtos);
        return StatusCode((int)apiResponse.StatusCode, apiResponse);
    }

    [HttpDelete]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Territories deleted", typeof(ApiResponse<bool>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "No codes provided")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No territories found for provided codes")]
    public async Task<IActionResult> DeleteMany([FromBody] IEnumerable<string> codes)
    {
        var apiResponse = await territoryService.DeleteManyByCodesAsync(codes);
        return StatusCode((int)apiResponse.StatusCode, apiResponse);
    }
}
