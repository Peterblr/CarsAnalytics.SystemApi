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
        var apiResponse = await territoryService.GetTerritoriesAsync(regionCode);
        return StatusCode((int)apiResponse.StatusCode, apiResponse);
    }

    // POST api/<TerritoriesController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
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
