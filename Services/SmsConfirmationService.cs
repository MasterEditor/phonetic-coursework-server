using Coursework_Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.Services
{
    public class SmsConfirmationService
    {
        public Dictionary<string, RegisterRequestModel> ids = new Dictionary<string, RegisterRequestModel>();
        public Dictionary<string, string> codes = new Dictionary<string, string>();

        public string Generate(RegisterRequestModel model)
        {
            string id = Guid.NewGuid().ToString("n").Substring(0, 8);

            ids.Add(id, model);

            string code = "8080";

            codes[id] = code;

            return id;
        }

        public RegisterRequestModel Check(string id, string p_code)
        {
            var code = codes.GetValueOrDefault(id);

            if (code is null || code != p_code) return null;

            var model = ids.GetValueOrDefault(id);

            if (model is null) return null;

            codes.Remove(id);
            ids.Remove(id);

            return model;
        }


    }
}
