using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Номер не может быть пустым")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Пароль не может быть пустым")]
        public string Password { get; set; }
    }
}
