using System;

namespace ProductRest.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        private Role[] Roles { get; set; }
    }

    public enum Role
    {
        User, 
        Admin
    }
}