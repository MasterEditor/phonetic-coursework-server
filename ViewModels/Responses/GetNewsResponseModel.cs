using Coursework_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels.Responses
{
    public class GetNewsResponseModel
    {
        public int TotalPages { get; set; }

        public List<News> News { get; set; }
    }
}
