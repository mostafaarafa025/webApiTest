using wabApi.Interfaces;
using wabApi.Models;

namespace wabApi.Repositories
{
    public class AccountRepository : IAccountRpository
    {
        ItiDbContext context;
        public AccountRepository(ItiDbContext _context) 
        { 
        this.context = _context;
        }
        public User addUser(User user)
        {
           context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public User getUserByEmail(string email)
        {
           return context.Users.FirstOrDefault(u=>u.Email == email);
        }

        public User getUserByRefreshToken(string refreshToken)
        {
            return context.Users.FirstOrDefault(u =>
              u.RefreshToken == refreshToken &&
              u.RefreshTokenExpiry > DateTime.Now);
        }

        public void save()
        {
            context.SaveChanges();
        }
    }
}
