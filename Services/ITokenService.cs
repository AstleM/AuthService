using AuthService.Dtos;

namespace AuthService.Services
{
    public interface ITokenService
    {
        public Task<TokenResponseDto> GenerateToken(TokenCreateDto loginDetails);
        public Task<UserResponseDto> GetUserFromTokenId(string tokenId);
        public Task<TokenResponseDto> RefreshToken(string tokenId);
    }
}
