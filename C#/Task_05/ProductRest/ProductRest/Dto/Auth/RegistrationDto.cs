using System.ComponentModel.DataAnnotations;

namespace ProductRest.Dto.Auth
{
    public class RegistrationDto
    {
        [Required] 
        [Display(Name = "First name")]
        public string FirstName { get; init; }
        
        [Required] 
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        
        [Required] 
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required] 
        [Display(Name = "Password")]
        [StringLength(40, ErrorMessage = "Password is too short (minimum is 8 characters).", MinimumLength =8)]
        public string Password { get; set; }
    }
}