using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels
{
    public class IsRegisteredRequestModel
    {
        [Required(ErrorMessage = "Номер указан неверно")]
        public string Number { get; set; }
    }
}
