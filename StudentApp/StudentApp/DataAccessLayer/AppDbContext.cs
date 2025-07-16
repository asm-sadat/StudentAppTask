using Microsoft.EntityFrameworkCore;
using StudentApp.Models;

namespace StudentApp.DataAccessLayer
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Courses> Courses { get; set; }
    }
}
