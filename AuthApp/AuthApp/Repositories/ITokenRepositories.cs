using AuthApp.Models;

namespace AuthApp.Repositories
{
    public interface ITokenRepositories
    {
        string CreateJWTToken(UserModel user);
    }
}
