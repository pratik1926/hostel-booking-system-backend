using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HostelManage.Application.DTOs.User
{
    public class UserCreateDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Contact { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}