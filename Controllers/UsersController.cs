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
            try
            {
                TokenResponseDto token = await tokenService.GenerateToken(loginDetails);

                if (token == null)
                    return NotFound();

                return Ok(token);
            } catch (Exception ex)
            {
                logger.LogError(ex, "Error during {method}", "GenerateToken");

                return BadRequest("There was an error");
            }
            
        }

        [HttpGet("/token/{tokenId}")]
        public async Task<ActionResult<UserResponseDto>> GetUserFromToken(string tokenId)
        {
            try
            {
                UserResponseDto user = await tokenService.GetUserFromTokenId(tokenId);

                if (user == null)
                    return NotFound();

                return Ok(user);
            } catch (Exception ex)
            {
                logger.LogError(ex, "Error during {method}", "GetUserFromToken");

                return BadRequest("There was an error");
            }
            
        }

        [HttpPost("/refreshToken/{tokenId}")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(string tokenId)
        {
            try
            {
                TokenResponseDto token = await tokenService.RefreshToken(tokenId);

                if (token == null)
                    return NotFound();

                return Ok(token);
            } catch (Exception ex)
            {
                logger.LogError(ex, "Error during {method}", "RefreshToken");

                return BadRequest("There was an error");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create(UserCreateDto userToCreate)
        {
            try
            {
                UserResponseDto user = await usersService.Create(userToCreate);

                return Ok(user);
            } 
            catch(ArgumentException ex)
            {
                logger.LogError(ex, "Error during {method}", "Create");

                return BadRequest(ex.Message);
            }
            catch(AlreadyExistsException ex)
            {
                logger.LogError(ex, "Error during {method}", "Create");

                return BadRequest("The email provided is already in use");
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error during {method}", "Create");

                return BadRequest("Something went wrong");
            }
            
        }
    }
}
