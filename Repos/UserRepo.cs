using AuthService.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace AuthService.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly IConfiguration configuration;

        public UserRepo(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<User> GetUserFromEmail(string email)
        {
            User user;

            using (IDbConnection db = new SqlConnection(configuration["DbConnectionString"]))
            {
                user = await db.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email", new {Email = email});
            }

            return user;
        }

        public async Task<User> SaveUser(User user)
        {
            User newUser;

            using (IDbConnection db = new SqlConnection(configuration["DbConnectionString"]))
            {
                newUser = await db.QuerySingleAsync<User>("INSERT INTO Users(Id, CreatedAt, UpdatedAt, Email, PasswordHash, Salt) OUTPUT INSERTED.* VALUES (@Id, @CreatedAt, @UpdatedAt, @Email, @PasswordHash, @Salt)", user);
            }

            return newUser;
        }
    }
}
