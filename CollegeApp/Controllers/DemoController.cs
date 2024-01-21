using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(PolicyName = "AllowOnlyGoogle")]
    public class DemoController : ControllerBase
    {
        private readonly ILogger<DemoController> _logger;
        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public ActionResult Index()
        {
            _logger.LogTrace("Log message from trace method");
            _logger.LogDebug("Log message from Debug method");
            _logger.LogInformation("Log message from Information method");
            _logger.LogWarning("Log message from Warning method");
            _logger.LogError("Log message from Error method");
            _logger.LogCritical("Log message from Critical method");

            return Ok();
        }
    }
}
