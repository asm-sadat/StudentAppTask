using AuthApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.DataAccessLayer
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options)
        {
            
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
