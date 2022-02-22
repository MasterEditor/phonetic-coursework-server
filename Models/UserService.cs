using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.Models
{
    public class UserService
    {
        public int Id { get; set; }

        public int UsedMinutes { get; set; }

        public int UsedInternet { get; set; }

        public int UsedSMS { get; set; }


        public int ServiceId { get; set; }

        public Service Service { get; set; }


        public int UserId { get; set; }

        public User User { get; set; }
    }
}
