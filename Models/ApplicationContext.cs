using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<UserTariff> UserTariffs { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Consumption> Consumptions { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<UserService> UserServices { get; set; }
        public DbSet<Statistic> Statistics { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


    }
}
