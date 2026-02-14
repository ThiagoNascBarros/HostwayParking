using HostwayParking.Business.UseCase.Session;
using HostwayParking.Business.UseCase.Session.Check_Out;
using HostwayParking.Business.UseCase.Session.List_Active;
using HostwayParking.Communication.Request;
using Microsoft.AspNetCore.Mvc;

namespace HostwayParking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionParkingController : ControllerBase
    {
        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] RequestRegisterCheckInJson request, [FromServices] ICheckInSessionUseCase useCase)
        {
            try
            {
                await useCase.Execute(request.Plate, request.Model, request.Color);
                return Created();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] RequestRegisterCheckOutJson request, [FromServices] ICheckOutUseCase useCase)
        {
            try
            {
                var result = await useCase.Execute(request.Plate);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices] IGetAllActiveSessionsUseCase useCase)
        {
            var result = await useCase.Execute();
            return Ok(result);
        }

        [HttpGet("checkout/preview/{plate}")]
        public async Task<IActionResult> CheckOutPreview([FromRoute] string plate, [FromServices] IGetCheckOutPreviewUseCase useCase)
        {
            try
            {
                var result = await useCase.Execute(plate);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
