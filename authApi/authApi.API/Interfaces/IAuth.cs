using System.Threading.Tasks;
using authApi.API.Models;

namespace authApi.API.Interfaces
{
    public interface IAuth
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> UserExists(string username);
    }
}