using MeterReadingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeterReadingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingController(IMeterReadingService meterReadingService)
        {
            _meterReadingService =  meterReadingService;
        }
        
        [HttpPost("meter-reading-uploads")]
        public async Task<IActionResult> UploadMeterReadings(IFormFile file)
        {
            var serviceResult = await _meterReadingService.ProcessMeterReadingCsv(file);

            if(!serviceResult.IsValid)
            {
                return BadRequest(serviceResult.ErrorMessage);
            }

            return Ok("File processed successfully.");
        }
    }

}

