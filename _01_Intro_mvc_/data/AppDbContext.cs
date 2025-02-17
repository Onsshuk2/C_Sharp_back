using _01_Intro_mvc_.Models;
using Microsoft.EntityFrameworkCore;
namespace _01_Intro_mvc_.data


{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
           : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
