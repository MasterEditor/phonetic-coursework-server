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

namespace Coursework_Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PromotionsController : Controller
    {
        private ApplicationContext db;

        public PromotionsController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> GetList()
        {
            var list = await db.Promotions.ToListAsync();

            return Ok(list);
        }

        public async Task<IActionResult> Add(Promotion promotion)
        {
            var p = await db.Promotions.FirstOrDefaultAsync(p => p.Id == promotion.Id || p.ImagePath == promotion.ImagePath);

            if (p != null) return BadRequest(new { Error = "Такая акция уже существует" });

            await db.Promotions.AddAsync(promotion);

            await db.SaveChangesAsync();

            return Ok();

        }

        public async Task<IActionResult> Remove(GetByIdRequestModel model)
        {
            var p = await db.Promotions.FirstOrDefaultAsync(p => p.Id == model.Id);

            if (p is null) return BadRequest(new { Error = "Такого пользователя не существует" });

            db.Promotions.Remove(p);

            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
