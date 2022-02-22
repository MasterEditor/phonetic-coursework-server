using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string ImagePath { get; set; }

        public News Simple()
        {
            return new News
            {
                Id = Id,
                Header = Header,
                Content = Content.Length > 100 ? Content.Substring(0, 100) : Content,
                Date = Date,
                ImagePath = ImagePath
            };
        }
    }
}
