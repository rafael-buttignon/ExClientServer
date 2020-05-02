using Microsoft.EntityFrameworkCore;
using Cliente.Models;

namespace Data.DbAccess
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Dados> Dados { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=localhost,1433;database=pitest;User ID=SA;Password=1q2w3e4r!@#$");
        }
    }
}