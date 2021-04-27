using System;
using Entities;

namespace Dto
{
    public class UserResultDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Role Roles { get; set; }
    }
}