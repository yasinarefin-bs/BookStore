
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace BookStpre.Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions options) : base(options)
        {
            
        }

        // public DbSet<Employee> Employees { get; set; }

       // public DbSet<Room> Rooms { get; set; }


    }
}
