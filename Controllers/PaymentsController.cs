using Coursework_Server.Models;
using Coursework_Server.Services;
using Coursework_Server.ViewModels.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentsController : Controller
    {
        private ValidationService _validationService;
        private ApplicationContext db;

        public PaymentsController(ValidationService validationService, ApplicationContext context)
        {
            _validationService = validationService;
            db = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Recharge(RechargeBalanceRequestModel model)
        {
            var phone = User.Identity.Name;

            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            if (model.Value < 1) return BadRequest(new { Error = "Некорректная сумма для пополнения" });
            if (model.CardMonth < 1 || model.CardMonth > 12) return BadRequest(new { Error = "Некорректная дата" });
            if (model.CardYear < 1 || model.CardYear> 99) return BadRequest(new { Error = "Некорректная дата" });
            if (model.CardCVV < 100 || model.CardCVV > 999) return BadRequest(new { Error = "Некорректный CVV код" });

            var user = await db.Users.Where(u => u.Number == phone).FirstOrDefaultAsync();

            var operation = new Operation()
            {
                Date = DateTime.Now,
                Source = $"Пополнение {model.CardNumber.Substring(0, 4)} ****",
                Value = model.Value,
                User = user
            };

            user.Balance += model.Value;

            user.Operations.Add(operation);

            await db.Operations.AddAsync(operation);

            await db.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetList()
        {
            var phone = User.Identity.Name;

            //var user = await db.Users.Include(u => u.Operations).FirstOrDefaultAsync(u => u.Number == phone);

            var user = await db.Users.Include(u => u.Operations).Where(u => u.Number == phone).FirstOrDefaultAsync();

            return Ok(user.Operations);
        }
    }
}
