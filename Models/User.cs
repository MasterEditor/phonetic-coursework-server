using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Password { get; set; }

        public int Minutes { get; set; }

        public int Internet { get; set; }

        public int SMS { get; set; }

        public double Balance { get; set; }

        public UserTariff UserTariff { get; set; }

        public List<Operation> Operations { get; set; } = new List<Operation>();

        public List<Consumption> Consumptions { get; set; } = new List<Consumption>();

        public List<UserService> UserServices { get; set; } = new List<UserService>();

        public string Role { get; set; }
    }
}
