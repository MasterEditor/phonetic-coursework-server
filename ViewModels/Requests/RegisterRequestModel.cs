using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "Неверный номер телефона")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Некорректный пароль")]
        public string Password { get; set; }
    }
}
