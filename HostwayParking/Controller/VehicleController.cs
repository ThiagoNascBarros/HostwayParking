using HostwayParking.Business.UseCase.Vehicle.Create;
using HostwayParking.Business.UseCase.Vehicle.GetAll;
using HostwayParking.Business.UseCase.Vehicle.Update;
using HostwayParking.Communication.Request;
using Microsoft.AspNetCore.Mvc;

namespace HostwayParking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestRegisterVehicleJson request, [FromServices] ICreateVehicleUseCase useCase)
        {
            try
            {
                await useCase.Execute(request);
                return Created();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{plate}")]
        public async Task<IActionResult> Update([FromRoute] string plate, [FromBody] RequestUpdateVehicleJson request, [FromServices] IUpdateVehicleUseCase useCase)
        {
            try
            {
                await useCase.Execute(plate, request);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices] IGetAllVehiclesUseCase useCase)
        {
            var result = await useCase.Execute();
            return Ok(result);
        }
    }
}
