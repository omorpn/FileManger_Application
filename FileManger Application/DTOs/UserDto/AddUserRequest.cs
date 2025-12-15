using FileManger_Application.Helpers;
using FileManger_Application.Model;
using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.DTOs.UserDto
{
    public class AddUserRequest
    {
        [Required(ErrorMessage = "First name cannot be empty.")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name cannot be empty.")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email address cannot be empty.")]
        [StringLength(70)]
        [EmailAddress(ErrorMessage = "Email address format is invalid.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password cannot be empty.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }


    public static class Extension
    {
        public static ApplicationUser ToUser(this AddUserRequest request)
        {
            return new ApplicationUser()
            {
                FirstName = Normalization.NormalizeName(request.FirstName),
                Email = Normalization.NormalizeEmail(request.Email),
                UserName = Normalization.NormalizeEmail(request.Email),
                LastName = Normalization.NormalizeName(request.LastName),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,

            };
        }
    }

}