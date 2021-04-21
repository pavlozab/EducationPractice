using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Dto;
using Entities;
using Microsoft.AspNetCore.Identity;

namespace Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<User> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task Delete(Guid id)
        {
            var existingUser = await _userManager.FindByIdAsync(id.ToString()); // _repository.GetOne(id);
            if (existingUser is null)
                throw new KeyNotFoundException("User hasn't been found");

            await _userManager.DeleteAsync(existingUser);
        }

        public async Task<User> Create(RegistrationDto registrationDto)
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                UserName = registrationDto.UserName,
                Email = registrationDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password),
            }; 
            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, "User");
            return user;
        }

        public async Task<UserResultDto> UpdateRoleOfUser(Guid id, Role newRole)
        {
            // var existingUser = await _repository.GetOne(id);
            // if (existingUser is null)
            //     throw new KeyNotFoundException("User hasn't been found");
            //
            // existingUser.Roles = newRole;
            //
            // await _repository.Update(existingUser);
            //
            return new UserResultDto
            {
                Id = id
            };
        }
        
        public async Task<IEnumerable<string>> GetUserRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}