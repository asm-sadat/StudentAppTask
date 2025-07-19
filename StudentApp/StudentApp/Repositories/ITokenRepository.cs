using Microsoft.AspNetCore.Identity;

namespace StudentApp.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
