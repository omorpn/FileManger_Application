using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.DTOs.UserDto
{
    public class UpdateUser
    {
        [Required]
        public string UserId { get; set; }
        [Required(ErrorMessage = "{0} cant be null or Empty"), StringLength(50), Display(Name = "First Name")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "{0} cant be null or Empty"), StringLength(50), Display(Name = "Last Name")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} cant be null or Empty"), StringLength(50), Display(Name = "Last Name")]

        public string LastName { get; set; }
        public string PhoneNumber { get; internal set; }
    }
}
