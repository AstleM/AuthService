using AuthService.Dtos;
using AuthService.Models;

namespace AuthService.Maps
{
    public interface ITokenMap
    {
        public TokenResponseDto GetTokenResponseDtoFromToken(Token token);
    }
}
