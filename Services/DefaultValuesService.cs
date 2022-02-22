using Coursework_Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.Services
{
    public class DefaultValuesService
    {

        public async Task SetDefaultTariff(ApplicationContext db, User user)
        {
            var t = await db.Tariffs.FirstOrDefaultAsync(t => t.Name == "Стандартный");

            //if (t is null) await db.Tariffs.AddAsync(new Tariff { Name = "Стандартный", Internet = 50, Minutes = 50, SMS = 50, Type = TariffType.Special, Price = 2.49 });

            UserTariff userTariff = new UserTariff
            {
                Tariff = t,
                User = user
            };

            await db.UserTariffs.AddAsync(userTariff);
        }

        public void SetDefaultTariffNonAsync(ApplicationContext db, User user)
        {
            var t = db.Tariffs.FirstOrDefault(t => t.Name == "Стандартный");

            //if (t is null) await db.Tariffs.AddAsync(new Tariff { Name = "Стандартный", Internet = 50, Minutes = 50, SMS = 50, Type = TariffType.Special, Price = 2.49 });

            UserTariff userTariff = new UserTariff
            {
                Tariff = t,
                User = user
            };

            db.UserTariffs.Add(userTariff);
        }
    }
}
