using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DbClasses
{
    public class AppDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Host=localhost;Database=my_database;Username=postgres;Password=my_password";

            optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        }

    }
}
