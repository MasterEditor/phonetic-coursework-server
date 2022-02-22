using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coursework_Server.Models
{
    public class Tariff
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int Minutes { get; set; }

        public int Internet { get; set; }

        public int SMS { get; set; }

        public double Price { get; set; }

        public TariffType Type { get; set; }

        [JsonIgnore]
        public List<UserTariff> UserTariffs { get; set; } = new List<UserTariff>();

        //public List<User> Users { get; set; } = new List<User>();

        public Tariff Simple()
        {
            return new Tariff
            {
                Id = Id,
                Name = Name,
                Minutes = Minutes,
                Internet = Internet,
                SMS = SMS,
                Price = Price,
                Type = Type
            };
        }
    }

    public enum TariffType
    {
        ForSmartphone = 0,
        ForInternet = 1,
        ForCalls = 2,
        Special = 3
    }
}
