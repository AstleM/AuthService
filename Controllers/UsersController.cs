using AuthService.Dtos;
using AuthService.Exceptions;
using AuthService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly ITokenService tokenService;
        private readonly ILogger<UsersController> logger;

        public UsersController(IUsersService usersService, ITokenService tokenService, ILogger<UsersController> logger)
        {
            this.usersService = usersService;
            this.tokenService = tokenService;
            this.logger = logger;
        }

        [HttpPost("/generateToken")]
        public async Task<ActionResult<TokenResponseDto>> GenerateToken(TokenCreateDto loginDetails)
        {
            TokenResponseDto token = await tokenService.GenerateToken(loginDetails);

            if (token == null)
                return NotFound();

            return Ok(token);
        }

        [HttpGet("/token/{tokenId}")]
        public async Task<ActionResult<UserResponseDto>> GetUserFromToken(string tokenId)
        {
            UserResponseDto user = await tokenService.GetUserFromTokenId(tokenId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("/refreshToken/{tokenId}")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(string tokenId)
        {
            TokenResponseDto token = await tokenService.RefreshToken(tokenId);

            if (token == null)
                return NotFound();

            return Ok(token);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create(UserCreateDto userToCreate)
        {
            try
            {
                logger.LogInformation("Create User Called for email: {email}", userToCreate.Email);
                UserResponseDto user = await usersService.Create(userToCreate);

                return Ok(user);
            } 
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(AlreadyExistsException ex)
            {
                return BadRequest("The email provided is already in use");
            }
            catch(Exception ex)
            {
                return BadRequest("Something went wrong");
            }
            
        }
    }
}
