using AuthService.Dtos;
using AuthService.Models;
using Mapster;

namespace AuthService.Maps
{
    public class TokenMap : ITokenMap
    {
        public TokenResponseDto GetTokenResponseDtoFromToken(Token token)
        {
            TokenResponseDto tokenResponseDto = token.Adapt<TokenResponseDto>();

            return tokenResponseDto;
        }
    }
}
