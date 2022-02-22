using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels
{
    public class ConfirmRegistrationRequestModel
    {
        [Required(ErrorMessage = "ID не может быть пустым")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Код не может быть пустым")]
        public string Code { get; set; }

    }
}
