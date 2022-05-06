using Microsoft.AspNetCore.Mvc;

namespace MyBooks.Controllers.V1
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("1.1")]
    [ApiVersion("1.2")]
    [ApiVersion("1.5")]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("get-test-data"), MapToApiVersion("1.0")]
        public IActionResult GetV1()
        {
            return Ok("This version V1.0");
        }
        [HttpGet("get-test-data")]
        public IActionResult GetV11()
        {
            return Ok("This is version V1.1");
        }
        [HttpGet("get-test-data"), MapToApiVersion("1.2")]
        public IActionResult GetV12()
        {
            return Ok("This is version V1.2");
        }
        [HttpGet("get-test-data"), MapToApiVersion("1.5")]
        public IActionResult GetV15()
        {
            return Ok("This is version V1.5");
        }
    }
}
