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
    public class ConsumptionsController : Controller
    {
        private Random rnd;
        private ValidationService _validationService;
        private ApplicationContext db;

        public ConsumptionsController(ValidationService validationService, ApplicationContext context)
        {
            _validationService = validationService;
            db = context;
            rnd = new Random();
        }
        
        [Authorize]
        public async Task<IActionResult> GetList()
        {
            var phone = User.Identity.Name;

            var user = await db.Users.Include(u => u.Consumptions).Where(u => u.Number == phone).FirstOrDefaultAsync();

            var count = user.Consumptions.Count();

            if(count < 10)
            {
                user.Consumptions.Clear();
                for(int i = 0; i < 10; i++)
                {
                    user.Consumptions.Add(new Consumption { Date = DateTime.Now.AddDays(-i), User = user, Value = rnd.Next(0, 100) });
                }
            }

            var cons = user.Consumptions.OrderBy(u => u.Date);

            await db.SaveChangesAsync();

            return Ok(cons);
        }

    }
}
