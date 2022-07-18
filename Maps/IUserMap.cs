using AuthService.Dtos;
using AuthService.Models;

namespace AuthService.Maps
{
    public interface IUserMap
    {
        public UserResponseDto GetUserResponseDtoFromUser(User user);
    }
}
