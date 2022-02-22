using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coursework_Server.Models
{
    public class Consumption
    {
        [JsonIgnore]
        public int Id { get; set; }

        public double Value { get; set; }

        public DateTime Date { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
