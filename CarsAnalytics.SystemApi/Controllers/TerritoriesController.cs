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

    // PUT api/<TerritoriesController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<TerritoriesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
