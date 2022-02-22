using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.Models
{
    public class UserTariff
    {
        public int Id { get; set; }

        public int UsedMinutes { get; set; }

        public int UsedInternet { get; set; }

        public int UsedSMS { get; set; }


        public int TariffId { get; set; }

        public Tariff Tariff { get; set; }

        
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
