using HostwayParking.Business.UseCase.Parking.GetAll;
using HostwayParking.Business.UseCase.Parking.Register;
using HostwayParking.Communication.Request;
using Microsoft.AspNetCore.Mvc;

namespace HostwayParking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Register([FromServices] IRegisterParkingUseCase useCase, [FromBody] RequestRegisterParkingJson request)
        {
            try
            {
                var result = await useCase.Execute(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices] IGetAllParkingUseCase useCase)
        {
            try
            {
                var result = await useCase.Execute();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}