using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadFile_Mini.Services;

namespace ReadFile_Mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecurringJobController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public RecurringJobController(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

       

        [HttpPost("schedule")]
        public IActionResult ScheduleJob()
        {
            RecurringJob.AddOrUpdate<DataTransferJob>("transfer-data-job",  job => job.TransferData(), "*/5 * * * *");
            return Ok("Job scheduled successfully!");
        }

       
    }
}
