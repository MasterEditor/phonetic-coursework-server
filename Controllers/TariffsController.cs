using Coursework_Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coursework_Server.ViewModels.Requests;
using Microsoft.EntityFrameworkCore;
using Coursework_Server.Services;
using Coursework_Server.ViewModels;

namespace Coursework_Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TariffsController : Controller
    {
        private ApplicationContext db;
        private ValidationService _validationService;
        private DefaultValuesService _defaultValuesService;

        public TariffsController(ApplicationContext context, ValidationService validationService, DefaultValuesService defaultValuesService)
        {
            db = context;
            _validationService = validationService;
            _defaultValuesService = defaultValuesService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Get()
        {
            var phone = User.Identity.Name;

            var user = await db.Users.Where(u => u.Number == phone).FirstOrDefaultAsync();

            var userTariff = await db.UserTariffs.Include(u => u.Tariff).Where(t => t.User == user).FirstOrDefaultAsync();

            //var tariff = await db.Tariffs.Where(t => t.Users.Contains(user)).FirstOrDefaultAsync();

            if (userTariff is null)
            {
                await _defaultValuesService.SetDefaultTariff(db, user);
                await db.SaveChangesAsync();
                userTariff = await db.UserTariffs.Include(u => u.Tariff).Where(t => t.User == user).FirstOrDefaultAsync();
            }

            return Ok(userTariff.Tariff);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetList(GetTariffsRequestModel model)
        {
            var phone = User.Identity.Name;

            var user = await db.Users.Where(u => u.Number == phone).FirstOrDefaultAsync();

            var userTariff = await db.UserTariffs.FirstOrDefaultAsync(t => t.User == user);

            var tariffs = await db.Tariffs.Where(t => t.Type == model.Type && !t.UserTariffs.Contains(userTariff)).ToListAsync();

            List<Tariff> simpleItems = new List<Tariff>();

            foreach (var t in tariffs)
            {
                simpleItems.Add(t.Simple());
            }

            return Ok(simpleItems);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeTariff(GetByIdRequestModel model)
        {
            var phone = User.Identity.Name;

            var user = await db.Users.Include(u => u.UserTariff).Where(u => u.Number == phone).FirstOrDefaultAsync();

            var tariff = await db.Tariffs.Where(t => t.Id == model.Id).FirstOrDefaultAsync();

            if (user.Balance < tariff.Price) return BadRequest(new { Error = "Недостаточно средств на балансе" });

            db.UserTariffs.Remove(user.UserTariff);

            var userTariff = new UserTariff();
            userTariff.Tariff = tariff;
            userTariff.User = user;

            user.UserTariff = userTariff;

            user.Balance -= tariff.Price;

            var operation = new Operation()
            {
                Date = DateTime.Now,
                Source = $"Тариф: {tariff.Name}",
                Value = -tariff.Price,
                User = user
            };

            await db.Operations.AddAsync(operation);

            await db.UserTariffs.AddAsync(userTariff);

            await db.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            var tariffs = await db.Tariffs.ToListAsync();

            return Ok(tariffs);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Remove(GetByIdRequestModel model)
        {
            var tariff = await db.Tariffs.FirstOrDefaultAsync(t => t.Id == model.Id);

            if (tariff is null) return BadRequest(new { Error = "Такого тарифа не существует" });

            db.Tariffs.Remove(tariff);

            await db.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(AddTariffRequestModel model)
        {
            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            var tariff = await db.Tariffs.FirstOrDefaultAsync(t => t.Id == model.Id);

            if (tariff is null) return BadRequest(new { Error = "Такого тарифа не существует" });

            if (model.Price < 0) return BadRequest(new { Error = "Цена не может быть меньше нуля" });

            tariff.Name = model.Name;
            tariff.Internet = model.Internet;
            tariff.Minutes = model.Minutes;
            tariff.Price = model.Price;
            tariff.SMS = model.SMS;
            tariff.Type = model.Type;

            await db.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(AddTariffRequestModel model)
        {
            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            if (model.Price < 0) return BadRequest(new { Error = "Цена не может быть меньше нуля" });

            var tariff = new Tariff();

            tariff.Name = model.Name;
            tariff.Internet = model.Internet;
            tariff.Minutes = model.Minutes;
            tariff.Price = model.Price;
            tariff.SMS = model.SMS;
            tariff.Type = model.Type;

            await db.Tariffs.AddAsync(tariff);

            await db.SaveChangesAsync();

            return Ok();
        }

    }
}
