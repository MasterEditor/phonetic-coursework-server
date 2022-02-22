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
using Coursework_Server.ViewModels.Responses;
using Coursework_Server.ViewModels.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Coursework_Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NewsController : Controller
    {
        private ApplicationContext db;
        private ValidationService _validationService;

        public NewsController(ApplicationContext context, ValidationService validationService)
        {
            db = context;
            _validationService = validationService;
        }

        [HttpPost]
        public async Task<IActionResult> GetList(GetNewsRequestModel model)
        {
            int page = 1;

            if (model.Page > 0) page = model.Page;

            IQueryable<News> source = db.News;

            if (!string.IsNullOrWhiteSpace(model.SearchString))
            {
                source = model.SearchType switch
                {
                    SearchType.Mixed => source.Where(n => (n.Header.Contains(model.SearchString) || n.Content.Contains(model.SearchString))),
                    SearchType.Header => source.Where(n => n.Header.Contains(model.SearchString)),
                    SearchType.Content => source.Where(n => n.Content.Contains(model.SearchString)),
                    _ => source
                };
            }

            if (model.From.HasValue) source = source.Where(n => n.Date > model.From.Value);
            if (model.To.HasValue) source = source.Where(n => n.Date < model.To.Value);

            var count = await source.CountAsync();
            var items = await source.OrderByDescending(i => i.Date).Skip((page - 1) * 10).Take(10).ToListAsync();

            var totalPages = (int)Math.Ceiling(count / (double)10);

            List<News> simpleItems = new List<News>();

            foreach (var i in items)
            {
                simpleItems.Add(i.Simple());
            }

            return Ok(new GetNewsResponseModel() { TotalPages = totalPages, News = simpleItems });
        }

        [HttpPost]
        public async Task<IActionResult> Get(GetNewsByIdRequestModel model)
        {
            var news = await db.News.FirstOrDefaultAsync(n => n.Id == model.Id);

            if (news == null) return BadRequest(new { Error = "Такой статьи не существует" });

            return Ok(news);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Remove(GetNewsByIdRequestModel model)
        {
            var news = await db.News.FirstOrDefaultAsync(n => n.Id == model.Id);

            if (news == null) return BadRequest(new { Error = "Такой статьи не существует" });

            db.News.Remove(news);

            await db.SaveChangesAsync();

            return Ok();
        } 

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(AddArticleRequestModel model)
        {
            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            var article = new News { Header = model.Header, Content = model.Content, ImagePath = model.ImagePath, Date = DateTime.Now };

            await db.News.AddAsync(article);

            await db.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(AddArticleRequestModel model)
        {
            var error = _validationService.GetErrorOrDefault(model);
            if (error != null) return BadRequest(new { Error = error });

            var article = await db.News.FirstOrDefaultAsync(a => a.Id == model.Id);

            if (article is null) return BadRequest(new { Error = "Новость не была найдена" });

            article.Header = model.Header;
            article.Content = model.Content;
            article.ImagePath = model.ImagePath;

            await db.SaveChangesAsync();

            return Ok();
        }

        private DateTime GenRandDate()
        {
            Random rand = new Random();
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rand.Next(range));
        }
    }
}
