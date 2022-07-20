using AuthService.Dtos;
using AuthService.Enums;
using AuthService.Exceptions;
using AuthService.Maps;
using AuthService.Models;
using AuthService.Repos;

namespace AuthService.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUsersService usersService;
        private readonly IIdService idService;
        private readonly ITokenRepo tokenRepo;
        private readonly ITokenMap tokenMap;
        private readonly IUserMap userMap;

        public TokenService(IUsersService usersService, IIdService idService, ITokenRepo tokenRepo, ITokenMap tokenMap, IUserMap userMap)
        {
            this.usersService = usersService;
            this.idService = idService;
            this.tokenRepo = tokenRepo;
            this.tokenMap = tokenMap;
            this.userMap = userMap;
        }

        public async Task<TokenResponseDto> GenerateToken(TokenCreateDto loginDetails)
        {
            User user = await usersService.ValidateUser(loginDetails);

            if (user == null)
                return null;

            string id = idService.GenerateId();

            Token token = new Token
            {
                Id = id,
                UserId = user.Id,
                CreatedAt = DateTime.Now,
                ValidUntil = DateTime.Now.AddHours(1),
                TokenType = TokenType.AuthenticationToken,
                HasBeenInvalidated = false
            };

            Token newToken = await tokenRepo.CreateToken(token);

            TokenResponseDto tokenResponse = tokenMap.GetTokenResponseDtoFromToken(newToken);

            return tokenResponse;
        }

        public async Task<UserResponseDto> GetUserFromTokenId(string tokenId)
        {
            Token currentToken = await tokenRepo.GetToken(tokenId);

            if (currentToken == null || currentToken.HasBeenInvalidated)
                return null;

            if (currentToken.ValidUntil < DateTime.Now)
            {
                throw new TokenExpiredException("Token has expired");
            }

            User user = await tokenRepo.GetUserFromTokenId(tokenId);

            if (user == null)
                return null;

            UserResponseDto userResponse = userMap.GetUserResponseDtoFromUser(user);

            return userResponse;
        }

        public async Task<TokenResponseDto> RefreshToken(string tokenId)
        {
            DateTime newValidUntil = DateTime.Now.AddHours(1);

            Token currentToken = await tokenRepo.GetToken(tokenId);

            if (currentToken == null || currentToken.HasBeenInvalidated)
                return null;

            if (currentToken.ValidUntil < DateTime.Now)
            {
                throw new TokenExpiredException("Token has already expired");
            }
                

            Token token = await tokenRepo.RefreshToken(tokenId, newValidUntil);

            if (token == null)
                return null;

            TokenResponseDto tokenResponse = tokenMap.GetTokenResponseDtoFromToken(token);

            return tokenResponse;
        }
    }
}
