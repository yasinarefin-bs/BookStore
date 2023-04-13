
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace BookStpre.Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }


    }
}
