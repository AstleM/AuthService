﻿using AuthService.Models;

namespace AuthService.Repos
{
    public interface ITokenRepo
    {
        public Task<Token> CreateToken(Token token);
        public Task<User> GetUserFromTokenId(string tokenId);
        public Task<Token> RefreshToken(string tokenId, DateTime newValidUntil);
    }
}
