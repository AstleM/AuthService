using AuthService.Models;

namespace AuthService.Repos
{
    public interface IUserRepo
    {
        public Task<User> SaveUser(User user);
        public Task<User> GetUserFromEmail(string email);
    }
}
