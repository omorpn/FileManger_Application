using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.Model
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required(ErrorMessage = "{0} cant be null or Empty"), StringLength(50), Display(Name = "First Name")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "{0} cant be null or Empty"), StringLength(50), Display(Name = "Last Name")]

        public string LastName { get; set; }

    }
}
