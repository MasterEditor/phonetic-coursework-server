using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels
{
    public class AddArticleRequestModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Заголовок не может быть пустым")]
        public string Header { get; set; }
        [Required(ErrorMessage = "Текст не может быть пустым")]
        public string Content { get; set; }
        public string ImagePath { get; set; }
    }
}
