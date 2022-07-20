using AuthService.Dtos;
using AuthService.Enums;
using AuthService.Exceptions;
using AuthService.Maps;
using AuthService.Models;
using AuthService.Repos;
using EmailValidation;

namespace AuthService.Services
{
    public class UserService : IUsersService
    {
        private readonly IPasswordService passwordService;
        private readonly IConfiguration configuration;
        private readonly IIdService idService;
        private readonly IUserRepo userRepo;
        private readonly IUserMap userMap;
        private readonly ITokenRepo tokenRepo;

        public UserService(IPasswordService passwordService, IConfiguration configuration, IIdService idService, IUserRepo userRepo, IUserMap userMap, ITokenRepo tokenRepo)
        {
            this.passwordService = passwordService;
            this.configuration = configuration;
            this.idService = idService;
            this.userRepo = userRepo;
            this.userMap = userMap;
            this.tokenRepo = tokenRepo;
        }

        private static int saltLength = 50;
        private static int passwordLength = 50;
        private static int hashIterations = 10000;

        public async Task<UserResponseDto> Create(UserCreateDto userToCreate)
        {

            try
            {
                bool emailIsValid = EmailValidator.Validate(userToCreate.Email);
                if (!emailIsValid)
                    throw new ArgumentException("Email is invalid");
            }
            catch
            {
                throw new ArgumentException("Email is invalid");
            }

            PasswordRequirements passwordRequirements = new PasswordRequirements();
            passwordRequirements.CannotContainStrings = new string[] {"password"};

            bool passwordIsValid = passwordService.ValidatePassword(userToCreate.Password, passwordRequirements);

            User existingUser = await userRepo.GetUserFromEmail(userToCreate.Email);

            if (existingUser != null)
                throw new AlreadyExistsException("User with that email already exists");

            byte[] salt = passwordService.GenerateSalt(saltLength);

            byte[] passwordHash = passwordService.HashPassword(userToCreate.Password, salt, hashIterations, passwordLength);

            string id = idService.GenerateId();

            User user = new User
            {
                Id = id,
                Email = userToCreate.Email,
                PasswordHash = passwordHash,
                Salt = salt,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmailConfirmed = false
            };

            User newUser = await userRepo.SaveUser(user);

            UserResponseDto userResponse = userMap.GetUserResponseDtoFromUser(newUser);

            return userResponse;
        }

        public async Task<User> ValidateUser(TokenCreateDto loginDetails)
        {
            User user = await userRepo.GetUserFromEmail(loginDetails.Email);

            if(user == null) return null;

            bool passwordsMatch = passwordService.CheckPasswordValid(loginDetails.Password, user.PasswordHash, user.Salt, hashIterations, passwordLength);

            if(!passwordsMatch) return null;

            return user;
        }

        public async Task SendEmailConfirmation(string tokenId)
        {
            Token token = await tokenRepo.GetToken(tokenId);

            if (token == null || token.HasBeenInvalidated)
                throw new ArgumentException("Token invalid");

            if (token.ValidUntil < DateTime.Now)
                throw new ArgumentException("Token is expired");

            User user;

            user = await tokenRepo.GetUserFromTokenId(tokenId);

            if (user == null)
                throw new ArgumentException("User not found");

            Token emailToken = new Token
            {
                Id = idService.GenerateId(),
                UserId = user.Id,
                CreatedAt = DateTime.Now,
                ValidUntil = DateTime.Now.AddHours(1),
                TokenType = TokenType.EmailConfirmationToken,
                HasBeenInvalidated = false
            };

            await tokenRepo.CreateToken(emailToken);
        }

        public async Task ConfirmEmail(string emailTokenId)
        {
            Token token = await tokenRepo.GetToken(emailTokenId);

            if (token == null || token.HasBeenInvalidated)
                throw new ArgumentException("Token invalid");

            if (token.ValidUntil < DateTime.Now)
                throw new ArgumentException("Token is expired");

            User user;

            user = await tokenRepo.GetUserFromTokenId(emailTokenId);

            if (user == null)
                throw new ArgumentException("User not found");

            await userRepo.SetUserEmailConfirmed(user.Id);
            await tokenRepo.SetTokenInvalid(emailTokenId);
        }
    }
}
