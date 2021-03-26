using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using DevOne.Security.Cryptography.BCrypt;
using Microsoft.Extensions.Logging;
using ProductRest.Controllers;
using ProductRest.Data.Contracts;
using ProductRest.Dtos;
using ProductRest.Entities;

namespace ProductRest.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> Registration(RegistrationDto registrationDto)
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                Email = registrationDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password)
            };
            
            await _repository.CreateUserAsync(user);

            return user;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _repository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return "";
            }

            if (BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return "";
            }
            
        }
    }
}