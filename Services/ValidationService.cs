using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.Services
{
    public class ValidationService
    {
        public string GetErrorOrDefault(object obj)
        {
            string error = null;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(obj);
            if (!Validator.TryValidateObject(obj, context, results))
            {
                error = results[0].ErrorMessage;
            }

            return error;
        }
    }
}
