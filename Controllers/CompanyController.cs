using AuthService.Dtos;
using AuthService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompanyResponseDto>>> Get()
        {
            List<CompanyResponseDto> companies = await companyService.GetAll();

            return Ok(companies);
        }

        [HttpGet("companies/{companyId}")]
        public async Task<ActionResult<CompanyResponseDto>> Get(string companyId)
        {
            CompanyResponseDto company = await companyService.Get(companyId);

            if (company == null)
                return NotFound();

            return Ok(company);
        }

        [HttpGet("companies/{companyId}/users")]
        public async Task<ActionResult<List<UserResponseDto>>> GetUsers(string companyId)
        {
            List<UserResponseDto> users = await companyService.GetUsers(companyId);

            if (users == null)
                return NotFound();

            return Ok(users);
        }

        [HttpGet("companies/{companyId}/applications")]
        public async Task<ActionResult<List<ApplicationResponseDto>>> GetApplications(string companyId)
        {
            List<ApplicationResponseDto> applications = await companyService.GetApplications(companyId);

            if (applications == null)
                return NotFound();

            return Ok(applications);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyResponseDto>> Create(CompanyCreateDto companyToCreate)
        {
            CompanyResponseDto company = await companyService.Create(companyToCreate);

            return Ok(company);
        }
    }
}
