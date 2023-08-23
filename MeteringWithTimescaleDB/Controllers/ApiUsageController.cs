using MeteringTest.Services;
using MeteringTest.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeteringTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsageController : ControllerBase
    {
        private readonly IApiUsageHandler _apiUsage;

        public ApiUsageController(IApiUsageHandler apiUsage)
        {
            _apiUsage = apiUsage;
        }

        [HttpGet()]
        [Route("ApiUsageMinutely")]
        public async Task<ActionResult<ApiUsageReport>> ApiUsagePerMinuteAsync([FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] string cust,
            CancellationToken cancellationToken)
        {
            if (from == DateTime.MinValue || to == DateTime.MinValue)
            {
                to = DateTime.UtcNow;
                from = to.AddMinutes(-30);
            }

            return await _apiUsage.GetUsage(from, to, cust, cancellationToken);
        }
    }
}
