using System.ComponentModel.DataAnnotations;

namespace FileManger_Application.Views.ViewsDTOs
{
    public record ResetPassword
    {
        [Required]
        public string Token { get; init; } = null!;
        [Required]
        public string Email { get; init; } = null!;
        [Required]

        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string NewPassword { get; set; }
    }
}
