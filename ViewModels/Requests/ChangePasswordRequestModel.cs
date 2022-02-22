using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels
{
    public class ChangePasswordRequestModel
    {
        [Required(ErrorMessage = "Пароль не может быть пустым")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "Пароль не может быть пустым")]
        public string PasswordToSet { get; set; }
    }
}
