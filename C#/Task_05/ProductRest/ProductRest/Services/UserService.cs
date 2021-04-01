using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Dto.Auth;
using ProductRest.Dto.User;
using ProductRest.Entities;
using ProductRest.Repositories.Contracts;
using ProductRest.Services.Contracts;

namespace ProductRest.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<User> GetUserByEmail(string email)
        {
            return await _repository.GetUserByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _repository.GetAllUsers();
        }

        public async Task DeleteUser(Guid id)
        {
            var existingUser = await _repository.GetUserAsync(id);
            if (existingUser is null)
                throw new KeyNotFoundException("User hasn't been found");

            await _repository.DeleteUser(id);
        }

        public async Task<User> CreateUser(RegistrationDto registrationDto)
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

        public async Task<UserResultDto> UpdateRoleOfUser(Guid id, Role newRole)
        {
            var existingUser = await _repository.GetUserAsync(id);
            if (existingUser is null)
                throw new KeyNotFoundException("User hasn't been found");
            
            existingUser.Roles = newRole;

            await _repository.UpdateUser(id, existingUser);

            return new UserResultDto
            {
                Id = id,
                Email = existingUser.Email,
                Roles = existingUser.Roles
            };
        }
    }
}