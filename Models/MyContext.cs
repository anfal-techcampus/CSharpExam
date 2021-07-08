using Microsoft.EntityFrameworkCore;

namespace CSharpExam.Models
{
    public class MyContext:DbContext
    {
        public MyContext(DbContextOptions options):base(options){}
        public DbSet<User> Users { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<Enthusiasts> Enthusiasts { get; set; }

    }
}