using Microsoft.AspNetCore.Mvc;

namespace Eyemergency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RfidController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetLatestTagId()
        {
            return Ok(new { tagId = SerialPortService.LatestTagId });
        }
    }
}
