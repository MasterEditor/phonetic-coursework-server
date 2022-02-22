using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coursework_Server.Models;
using Coursework_Server.ViewModels;
using Coursework_Server.Services;
using Microsoft.EntityFrameworkCore;
using Coursework_Server.ViewModels.Requests;
using Microsoft.AspNetCore.Authorization;

namespace Coursework_Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ServicesController : Controller
    {
        private ApplicationContext db;
        private ValidationService _validationService;

        public ServicesController(ApplicationContext context, ValidationService validationService)
        {
            db = context;
            _validationService = validationService;
        }

        [Authorize]
        public async Task<IActionResult> GetMyList()
        {
            var phone = User.Identity.Name;

            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == phone);

            var services = await db.UserServices.Where(us => us.User == user).Join(db.Services, us => us.ServiceId, s => s.Id, (us, s) => new { Service = us.Service }).ToListAsync();

            var items = new List<Service>();

            foreach(var service in services)
            {
                items.Add(service.Service);
            }

            return Ok(items);
        }

        [Authorize]
        public async Task<IActionResult> GetList(GetAllServicesRequestModel model)
        {
            var phone = User.Identity.Name;

            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == phone);

            IQueryable<Service> query = db.Services;

            if (!string.IsNullOrWhiteSpace(model.Header))
            {
                query = query.Where(s => s.Header == model.Header);
            }

            if (!string.IsNullOrWhiteSpace(model.SearchQuery))
            {
                query = query.Where(s => s.Name.Contains(model.SearchQuery) || s.Details.Contains(model.SearchQuery));
            }

            var services = await query.Where(s => !db.UserServices.Any(us => us.ServiceId == s.Id && us.UserId == user.Id)).ToListAsync();

            return Ok(services);
        }

        [Authorize]
        public async Task<IActionResult> Subscribe(GetByIdRequestModel model)
        {
            var phone = User.Identity.Name;

            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == phone);

            if (user is null) return BadRequest(new { Error = "Пользователь не найден" });

            var service = await db.Services.FirstOrDefaultAsync(s => s.Id == model.Id);

            if (service is null) return BadRequest(new { Error = "Услуга не найдена" });

            var isExists = await db.UserServices.AnyAsync(u => u.Service == service && u.User == user);

            if (isExists) return BadRequest(new { Error = "Услуга уже подключена" });

            if (user.Balance < service.Price) return BadRequest(new { Error = "Недостаточно денег на балансе" });

            var userService = new UserService()
            {
                Service = service,
                User = user
            };

            user.Balance -= service.Price;

            var operation = new Operation()
            {
                Date = DateTime.Now,
                Source = $"Услуга: {service.Name}",
                Value = -service.Price,
                User = user
            };

            await db.Operations.AddAsync(operation);

            await db.UserServices.AddAsync(userService);

            await db.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        public async Task<IActionResult> Unsubscribe(GetByIdRequestModel model)
        {
            var phone = User.Identity.Name;

            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == phone);

            if (user is null) return BadRequest(new { Error = "Пользователь не найден" });

            var service = await db.Services.FirstOrDefaultAsync(s => s.Id == model.Id);

            if (service is null) return BadRequest(new { Error = "Услуга не найдена" });

            var userService = await db.UserServices.FirstOrDefaultAsync(us => us.User == user && us.Service == service);

            if (userService is null) return BadRequest(new { Error = "Услуга не найдена" });

            db.UserServices.Remove(userService);

            await db.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            var services = await db.Services.ToListAsync();

            return Ok(services);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Remove(GetByIdRequestModel model)
        {
            var service = await db.Services.FirstOrDefaultAsync(s => s.Id == model.Id);

            if (service is null) return BadRequest(new { Error = "Такого сервиса не существует" });

            db.Services.Remove(service);

            await db.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(AddServiceRequestModel model)
        {
            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            if (model.Price < 0) return BadRequest(new { Error = "Цена не может быть меньше нуля" });

            var service = await db.Services.FirstOrDefaultAsync(s => s.Id == model.Id);

            if (service is null) return BadRequest(new { Error = "Такого сервиса не существует" });

            var dur = model.Duration.ToLower();

            if (dur == "infinite" || dur == "month" || dur == "day")
            {
                service.Name = model.Name;
                service.Header = model.Header;
                service.Internet = model.Internet;
                service.Minutes = model.Minutes;
                service.SMS = model.SMS;
                service.Price = model.Price;
                service.Details = model.Details;
                service.Duration = model.Duration;

                await db.SaveChangesAsync();

                return Ok();
            }
            return BadRequest(new { Error = "Некорректная продолжительность" });

            
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(AddServiceRequestModel model)
        {
            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            if (model.Price < 0) return BadRequest(new { Error = "Цена не может быть меньше нуля" });

            var dur = model.Duration.ToLower();

            if (dur == "infinite" || dur == "month" || dur == "day")
            {
                var service = new Service();

                service.Name = model.Name;
                service.Header = model.Header;
                service.Internet = model.Internet;
                service.Minutes = model.Minutes;
                service.SMS = model.SMS;
                service.Price = model.Price;
                service.Details = model.Details;

                await db.Services.AddAsync(service);

                await db.SaveChangesAsync();

                return Ok();
            }
            return BadRequest(new { Error = "Некорректная продолжительность" });
        }
    }
}
