using wabApi.Migrations;
using wabApi.Models;

namespace wabApi.Interfaces
{
    public interface IAccountRpository

    {
        User addUser(User user);
       User getUserByEmail(string email);
        User getUserByRefreshToken(string refreshToken);
        void save();
    }
}
