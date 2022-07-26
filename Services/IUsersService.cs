﻿using AuthService.Dtos;
using AuthService.Models;

namespace AuthService.Services
{
    public interface IUsersService
    {
        public Task<UserResponseDto> Create(UserCreateDto userToCreate);

        public Task<User> ValidateUser(TokenCreateDto loginDetails);
    }
}
