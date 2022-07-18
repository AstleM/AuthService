using AuthService.Dtos;

namespace AuthService.Services
{
    public interface ICompanyService
    {
        public Task<List<CompanyResponseDto>> GetAll();
        public Task<CompanyResponseDto> Get(string id);
        public Task<List<UserResponseDto>> GetUsers(string companyId);
        public Task<List<ApplicationResponseDto>> GetApplications(string companyId);
        public Task<CompanyResponseDto> Create(CompanyCreateDto companyToCreate);
    }
}
