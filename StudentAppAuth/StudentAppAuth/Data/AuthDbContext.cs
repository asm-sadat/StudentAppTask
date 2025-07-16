using Microsoft.EntityFrameworkCore;

namespace StudentAppAuth.Data
{
    public class AuthDbContext: DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext>options):base(options)
        {
            
        }
    }
}
