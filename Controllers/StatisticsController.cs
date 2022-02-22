using Coursework_Server.Models;
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
    public class StatisticsController : Controller
    {
        private Random rnd;
        private ApplicationContext db;

        public StatisticsController(ApplicationContext context)
        {
            db = context;
            rnd = new Random();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetList(GetByTypeRequestModel model)
        {
            var count = db.Statistics.Where(s => s.Type == model.Type).Count();

            if (count < 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    db.Statistics.Add(new Statistic { Date = DateTime.Now.AddDays(-i), Value = rnd.Next(0, 100), Type = model.Type });
                }
            }

            var cons = await db.Statistics.Where(s => s.Type == model.Type).OrderBy(u => u.Date).ToListAsync();

            await db.SaveChangesAsync();

            return Ok(cons);
        }

    }
}
