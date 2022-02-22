using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coursework_Server.ViewModels.Requests;
using Coursework_Server.Services;

namespace Coursework_Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FeedbackController : Controller
    {
        EmailService _emailService;

        public FeedbackController(EmailService emailService )
        {
            _emailService = emailService;
        }

        public async Task<IActionResult> Send(SendFeedbackRequestModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Message)) return BadRequest(new { Error = "Отзыв не может быть пустым" });

            await _emailService.SendEmailAsync("what", model.Message);

            return Ok();
        }
    }
}
