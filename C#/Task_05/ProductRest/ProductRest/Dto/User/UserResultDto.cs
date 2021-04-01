using System;
using ProductRest.Entities;

namespace ProductRest.Dto.User
{
    public class UserResultDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Role Roles { get; set; }
    }
}