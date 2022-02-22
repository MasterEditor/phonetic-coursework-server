using Coursework_Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels
{
    public class AddTariffRequestModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название не может быть пустым")]
        public string Name { get; set; }

        public int Minutes { get; set; }

        public int Internet { get; set; }

        public int SMS { get; set; }

        public double Price { get; set; }

        public TariffType Type { get; set; }
    }
}
