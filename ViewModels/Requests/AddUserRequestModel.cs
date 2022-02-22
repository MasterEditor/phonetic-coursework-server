using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Coursework_Server.ViewModels.Requests
{
    public class AddUserRequestModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Номер не может быть пустым")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Пароль не может быть пустым")]
        public string Password { get; set; }

        public double Balance { get; set; }
    }
}
