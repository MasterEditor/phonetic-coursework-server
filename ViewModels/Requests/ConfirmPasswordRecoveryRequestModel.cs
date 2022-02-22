using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels
{
    public class ConfirmPasswordRecoveryRequestModel : ConfirmRegistrationRequestModel
    {
        public string Password { get; set; }
    }

}
