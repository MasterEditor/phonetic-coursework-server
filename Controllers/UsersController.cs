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
using Microsoft.AspNetCore.Authorization;
using Coursework_Server.ViewModels.Requests;
using System.Text.RegularExpressions;

namespace Coursework_Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : Controller
    {
        private ValidationService _validationService;
        private SmsConfirmationService _smsConfirmationService;
        private TokenService _tokenService;
        private ApplicationContext db;
        private DefaultValuesService _defaultValuesService;

        public UsersController(ValidationService validationService, SmsConfirmationService smsConfirmationService, ApplicationContext context, TokenService tokenService, DefaultValuesService defaultValuesService)
        {
            _validationService = validationService;
            _smsConfirmationService = smsConfirmationService;
            _tokenService = tokenService;
            _defaultValuesService = defaultValuesService;

            db = context;

            if(!db.Users.Any(u => u.Role == "Admin"))
            {
                var user = new User { Number = "375000000000", Role = "Admin", Balance = 0, Password = "qwerty" };
                db.Users.Add(user);
                var t = db.Tariffs.FirstOrDefault(t => t.Name == "Стандартный");

                if (t is null) db.Tariffs.Add(new Tariff { Name = "Стандартный", Internet = 50, Minutes = 50, SMS = 50, Type = TariffType.Special, Price = 2.49 });

                _defaultValuesService.SetDefaultTariffNonAsync(db, user);

                db.SaveChanges();
            }
        }

        [HttpPost]
        public async Task<ActionResult> IsRegistered(IsRegisteredRequestModel model)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == model.Number);

            if (user is null) return NotFound();

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRequestModel model)
        {
            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == model.Number && u.Password == model.Password);
            if (user == null) return BadRequest(new { Error = "Неверный номер или пароль" });

            var token = _tokenService.CreateSecurityToken(model.Number, user.Role);

            return Ok(new { token = token, Role = user.Role });
        }

        [HttpPost]
        public async Task<ActionResult> RecoverPassword(RecoverPasswordRequestModel model)
        {
            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == model.Number);
            if (user == null) return BadRequest(new { Error = "Такого пользователя не существует" });

            var id = _smsConfirmationService.Generate(new RegisterRequestModel { Number = model.Number }) ;

            return Ok(new { id = id });
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmPasswordRecovery(ConfirmPasswordRecoveryRequestModel model)
        {
            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            var reg_data = _smsConfirmationService.Check(model.Id, model.Code);
            if (reg_data is null) return BadRequest(new { Error = "Неверный код" });

            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == reg_data.Number);
            if (user == null) return BadRequest(new { Error = "Такого пользователя не существует" });

            user.Password = model.Password;
            await db.SaveChangesAsync();

            var token = _tokenService.CreateSecurityToken(reg_data.Number, "User");

            return Ok(new { token = token, role = user.Role });

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel model)
        {
            var phone = User.Identity.Name;

            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            if (model.PasswordToSet.Length < 6) return BadRequest(new { Error = "Пароль слишком слабый" });

            if (model.PasswordToSet == model.CurrentPassword) return BadRequest(new { Error = "Пароли совпадают" });

            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == phone);

            if (user is null) return BadRequest(new { Error = "Пользователь не найден" });

            if (user.Password != model.CurrentPassword) return BadRequest(new { Error = "Текущий пароль не совпадает с реальным" });

            user.Password = model.PasswordToSet;

            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestModel model)
        {
            var error = _validationService.GetErrorOrDefault(model);
            if(error != null) return BadRequest(new { Error = error });

            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == model.Number);
            if (user != null) return BadRequest(new { Error = "Такой пользователь уже существует" });

            var pattern = @"375(\d*)";

            if (!Regex.IsMatch(model.Number, pattern, RegexOptions.IgnoreCase)) return BadRequest(new { Error = "Некорректный номер" });

            var id = _smsConfirmationService.Generate(model);

            return Ok(new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmRegistration(ConfirmRegistrationRequestModel model)
        {
            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            var reg_data = _smsConfirmationService.Check(model.Id, model.Code);
            if(reg_data is null) return BadRequest(new { Error = "Неверный код" });

            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == reg_data.Number);
            if (user != null) return BadRequest(new { Error = "Такой пользователь уже зарегестрирован" });

            user = new User { Number = reg_data.Number, Password = reg_data.Password, Role = "User"};

            var t = await db.Tariffs.FirstOrDefaultAsync(t => t.Name == "Стандартный");

            if (t is null) await db.Tariffs.AddAsync(new Tariff { Name = "Стандартный", Internet = 50, Minutes = 50, SMS = 50, Type = TariffType.Special, Price = 2.49 });

            await db.Users.AddAsync(user);

            await db.SaveChangesAsync();

            await _defaultValuesService.SetDefaultTariff(db, user);

            await db.SaveChangesAsync();

            var token =_tokenService.CreateSecurityToken(reg_data.Number, "User");

            return Ok(new { token = token, Role = user.Role });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetUserInfo()
        {
            var phone = User.Identity.Name;

            var user = await db.Users.FirstOrDefaultAsync(u => u.Number == phone);

            if (user is null) return BadRequest(new { Error = "Пользователь не найден" });

            var userTariff = await db.UserTariffs.Include(t => t.Tariff).FirstOrDefaultAsync(t => t.User == user);

            if (userTariff is null)
            {
                await _defaultValuesService.SetDefaultTariff(db, user);
                await db.SaveChangesAsync();
                userTariff = await db.UserTariffs.Include(t => t.Tariff).FirstOrDefaultAsync(t => t.User == user);
            }

            var servicesData = await db.UserServices.Where(us => us.User == user).Join(db.Services, us => us.ServiceId, u => u.Id, (us, s) => new
            {
                AllMinutes = s.Minutes,
                AllInternet = s.Internet,
                AllSMS = s.SMS,
                Minutes = us.UsedMinutes,
                Internet = us.UsedInternet,
                SMS = us.UsedSMS
            }).ToListAsync();

            int AllMinutes = userTariff.Tariff.Minutes;
            int AllInternet = userTariff.Tariff.Internet;
            int AllSMS = userTariff.Tariff.SMS;
            int Minutes = AllMinutes - userTariff.UsedMinutes;
            int Internet = AllInternet - userTariff.UsedInternet;
            int SMS = AllSMS- userTariff.UsedSMS;

            foreach (var s in servicesData)
            {
                AllMinutes += s.AllMinutes;
                AllInternet += s.AllInternet;
                AllSMS += s.AllSMS;
                Minutes += s.AllMinutes - s.Minutes;
                Internet += s.AllInternet - s.Internet;
                SMS += s.AllSMS - s.SMS;
            }


            var info = new
            {
                Number = user.Number,
                Minutes = Minutes,
                Internet = Internet,
                SMS = SMS,
                Balance = user.Balance,
                AllMinutes = AllMinutes,
                AllSMS = AllSMS,
                AllInternet = AllInternet
                
            };

            return Ok(info);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            var users = await db.Users.ToListAsync();

            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Remove(GetByIdRequestModel model)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == model.Id);

            if (user is null) return BadRequest("Такой пользователь не найден");

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
