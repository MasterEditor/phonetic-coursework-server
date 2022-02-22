using Coursework_Server.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StorageController : Controller
    {
        private IWebHostEnvironment _appEnvironment;
        
        public StorageController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        } 

        [HttpGet]
        public IActionResult GetImage(string fileName)
        {
            string filePath = Path.Combine(_appEnvironment.WebRootPath, "Images", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest(new { Error = "Файл не существует" });
            }

            FileStream fs = new FileStream(filePath, FileMode.Open);

            string fileType = "application/" + Path.GetExtension(filePath).Replace(".", "");

            return File(fs, fileType, fileName);

        }

        [HttpPost]
        public IActionResult GetAllImages()
        {
            string folder = Path.Combine(_appEnvironment.WebRootPath, "Images");


            List<string> AllImages= Directory.GetFiles(folder).ToList();

            for(int i = 0; i < AllImages.Count; i++)
            {
                AllImages[i] = Path.GetFileName(AllImages[i]);
            }

            return Ok(AllImages);
        }

        [HttpPost]
        public IActionResult RemoveImage(GetByNameRequestModel model)
        {
            string path = Path.Combine(_appEnvironment.WebRootPath, "Images", model.Name);

            if (!System.IO.File.Exists(path)) return BadRequest(false);

            System.IO.File.Delete(path);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(IFormFile image)
        {
            if (image != null)
            {
                string name = Guid.NewGuid().ToString("n").Substring(0, 8) + Path.GetExtension(image.FileName);
                string path = Path.Combine(_appEnvironment.WebRootPath, "Images", name);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return Ok();

            }

            return BadRequest(new { Error = "Файл пуст" });
        }
    }
}
