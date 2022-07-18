using AuthService.Dtos;
using AuthService.Models;
using Mapster;

namespace AuthService.Maps
{
    public class UserMap : IUserMap
    {
        public UserResponseDto GetUserResponseDtoFromUser(User user)
        {
            UserResponseDto userResponseDto = user.Adapt<UserResponseDto>();

            return userResponseDto;
        }
    }
}
