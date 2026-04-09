using System.ComponentModel.DataAnnotations;

namespace HostelManage.Application.DTOs
{
    public class HostelResetPasswordDTO
    {
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{6,}$",
            ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, and 1 special character.")]
        public string NewPassword { get; set; }
    }
}
