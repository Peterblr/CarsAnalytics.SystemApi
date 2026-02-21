using APIResponseWrapper;
using CarsAnalytics.SystemApi.Dto;
using CarsAnalytics.SystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarsAnalytics.SystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarModelController(ICarModelService service) : ControllerBase
    {
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns list of car models", typeof(ApiResponse<IEnumerable<CarModelDto>>))]
        public async Task<IActionResult> Get()
        {
            var apiResponse = await service.GetAllAsync();
            return StatusCode((int)apiResponse.StatusCode, apiResponse);
        }
    }
}
