using HostwayParking.Business.UseCase.Report.Occupancy;
using HostwayParking.Business.UseCase.Report.Revenue;
using HostwayParking.Business.UseCase.Report.TopVehicles;
using Microsoft.AspNetCore.Mvc;

namespace HostwayParking.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueByDay(
            [FromQuery] int days,
            [FromServices] IGetRevenueByDayUseCase useCase)
        {
            try
            {
                var result = await useCase.Execute(days);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("top-vehicles")]
        public async Task<IActionResult> GetTopVehiclesByTime(
            [FromQuery] DateTime start,
            [FromQuery] DateTime end,
            [FromServices] IGetTopVehiclesByTimeUseCase useCase)
        {
            try
            {
                var result = await useCase.Execute(start, end);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("occupancy")]
        public async Task<IActionResult> GetOccupancyByHour(
            [FromQuery] DateTime start,
            [FromQuery] DateTime end,
            [FromServices] IGetOccupancyByHourUseCase useCase)
        {
            try
            {
                var result = await useCase.Execute(start, end);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
