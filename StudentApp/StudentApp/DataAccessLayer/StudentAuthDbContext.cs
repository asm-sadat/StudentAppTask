using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentApp.DataAccessLayer
{
    public class StudentAuthDbContext: IdentityDbContext
    {
        public StudentAuthDbContext(DbContextOptions<StudentAuthDbContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var visitorRoleId = "d7581ef1-3fcb-47f4-8bcd-fd0ef5eaf556";
            var adminRoleId = "ed913db0-2cd7-4f8c-bb9a-7e26e6c798a1";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = visitorRoleId,
                    ConcurrencyStamp = visitorRoleId,
                    Name = "Visitor",
                    NormalizedName = "VISITOR"
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
