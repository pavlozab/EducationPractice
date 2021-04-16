using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class RegistrationDto
    {
        [Required] 
        [Display(Name = "First name")]
        [RegularExpression(@"[a-zA-z,.\s]+", ErrorMessage = "{0} must contain only letters ")]
        public string FirstName { get; set; }
        
        [Required] 
        [Display(Name = "Last name")]
        [RegularExpression(@"[a-zA-z,.\s]+", ErrorMessage = "{0} must contain only letters ")]
        public string LastName { get; set; }
        
        [Required] 
        [Display(Name = "UserName")]
        [RegularExpression(@"[a-zA-z,.\s]+", ErrorMessage = "{0} must contain only letters ")]
        public string UserName { get; set; }
        
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