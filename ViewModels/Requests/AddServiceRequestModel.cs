using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels.Requests
{
    public class AddServiceRequestModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Категория не может быть пуста")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Название не может быть пустым")]
        public string Name { get; set; }

        public string Details { get; set; }

        public double Price { get; set; }

        [Required(ErrorMessage = "Продолжительность не может отсутствовать")]
        public string Duration { get; set; }

        public int Minutes { get; set; }

        public int Internet { get; set; }

        public int SMS { get; set; }
    }
}
