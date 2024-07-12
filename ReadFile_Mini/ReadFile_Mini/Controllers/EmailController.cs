using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadFile_Mini.Requestes;
using ReadFile_Mini.Services;

namespace ReadFile_Mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     // Protect the entire controller
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        [Route("send")]
        [Authorize]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid email request");
            }

            await _emailService.SendEmailAsync(emailRequest.To, emailRequest.Subject, emailRequest.Body);
            return Ok("Email sent successfully");
        }
    }
}
