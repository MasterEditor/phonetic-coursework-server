using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.Models
{
    public class ContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=phoneticdb;Trusted_Connection=True;";

            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
