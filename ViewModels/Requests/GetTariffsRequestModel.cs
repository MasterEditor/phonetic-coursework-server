using Coursework_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels.Requests
{
    public class GetTariffsRequestModel
    {
        public TariffType Type { get; set; }
    }
}
