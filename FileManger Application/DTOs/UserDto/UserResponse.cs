using FileManger_Application.Model;
using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.DTOs.UserDto
{

    public class UserResponse
    {

        public Guid UserId { get; set; }
        [Required(ErrorMessage = "{0} cant be null or Empty"), StringLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "{0} cant be null or Empty"), StringLength(50), Display(Name = "Email Address")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "{0} cant be null or Empty"), StringLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public static class CustomExtension
    {

        public static UserResponse ToUserResponse(this ApplicationUser user)
        {
            return new UserResponse
            {
                UserId = user.Id
                ,
                FirstName = user.FirstName,
                LastName = user.LastName
                ,

                IsActive = user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.Now,

                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
            };

        }
    }
}
