using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels.Requests
{
    public class GetNewsRequestModel
    {
        public int Page { get; set; } = 1;

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public string SearchString { get; set; }

        public SearchType SearchType { get; set; }
    }

    public enum SearchType
    {
        Mixed,
        Header,
        Content
    }
}
