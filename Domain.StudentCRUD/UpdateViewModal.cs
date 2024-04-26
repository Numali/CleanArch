using System.ComponentModel.DataAnnotations;

namespace Domain.StudentCRUD
{
    public class UpdateViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; } 
        [Required]
        public string? Role { get; set; } // Property to specify the role of the user
    }
}
