using AuthService.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace AuthService.Repos
{
    public class TokenRepo: ITokenRepo
    {
        private readonly IConfiguration configuration;

        public TokenRepo(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Token> CreateToken(Token token)
        {
            Token newToken;

            using (IDbConnection db = new SqlConnection(configuration["DbConnectionString"]))
            {
                newToken = await db.QuerySingleAsync<Token>("INSERT INTO Tokens(Id, CreatedAt, UserId, ValidUntil) OUTPUT INSERTED.* VALUES (@Id, @CreatedAt, @UserId, @ValidUntil)", token);
            }

            return newToken;
        }

        public async Task<Token> GetToken(string tokenId)
        {
            Token token;

            using(IDbConnection db = new SqlConnection(configuration["DbConnectionString"]))
            {
                token = await db.QueryFirstOrDefaultAsync<Token>("SELECT * FROM Tokens WHERE Id = @Id", new { Id = tokenId });
            }

            return token;
        }

        public async Task<User> GetUserFromTokenId(string tokenId)
        {
            User user;

            using (IDbConnection db = new SqlConnection(configuration["DbConnectionString"]))
            {
                user = await db.QuerySingleOrDefaultAsync<User>("SELECT U.* FROM Users U INNER JOIN Tokens T On T.UserId = U.Id WHERE T.Id = @TokenId", new {TokenId = tokenId});
            }

            return user;
        }

        public async Task<Token> RefreshToken(string tokenId, DateTime newValidUntil)
        {
            Token token;

            using (IDbConnection db = new SqlConnection(configuration["DbConnectionString"]))
            {
                token = await db.QuerySingleOrDefaultAsync<Token>("UPDATE Tokens SET ValidUntil = @ValidUntil OUTPUT INSERTED.* WHERE Id = @Id AND ValidUntil >= @Now", new { ValidUntil = newValidUntil, Id = tokenId, Now = DateTime.Now });
            }

            return token;
        }
    }
}
