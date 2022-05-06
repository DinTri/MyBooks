using Microsoft.AspNetCore.Mvc;
using MyBooks.Data.Services;

namespace MyBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogsService _logsService;
        public LogsController(LogsService logsService)
        {
            _logsService = logsService;
        }

        [HttpGet("Get-all-logs-from-db")]
        public IActionResult GetAllLogsFromDb()
        {
            try
            {
                var allLogs = _logsService.GetAllLogsFromDb();
                return Ok(allLogs);
            }
            catch (Exception)
            {
                return BadRequest("Could not return any logs from DB");
            }

        }
    }
}
