using AuthService.Dtos;

namespace AuthService.Services
{
    public interface IApplicationService
    {
        public Task<ApplicationResponseDto> Get(string id);
    }
}
