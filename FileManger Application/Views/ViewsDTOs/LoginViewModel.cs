using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.Views.ViewsDTOs
{
    public record LoginViewModel
    {
        [Display(Name = "Email Address")]
        [EmailAddress]
        [Required(ErrorMessage = "{0} can't be null or empty ")]
        public string Email { get; init; } = default!;
        [Display(Name = "Password")]
        [Required, DataType(DataType.Password)]
        public string Password { get; init; } = default!;
        public bool RememberMe { get; init; }
    }
}
