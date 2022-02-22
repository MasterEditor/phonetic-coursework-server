using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coursework_Server.Models
{
    public class Service
    {
        public int Id { get; set; }

        public string Header { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }

        public double Price { get; set; }

        public string Duration { get; set; } = "Infinite";

        public int Minutes { get; set; }

        public int Internet { get; set; }

        public int SMS { get; set; }

        [JsonIgnore]
        public List<UserService> UserServices { get; set; } = new List<UserService>();

    }

}
