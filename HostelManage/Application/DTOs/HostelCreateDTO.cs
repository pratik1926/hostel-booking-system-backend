using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HostelManage.Application.DTOs
{
    public class HostelCreateDTO
    {
        [Required(ErrorMessage = "Hostel name is required.")]
        [StringLength(100, ErrorMessage = "Hostel name cannot exceed 100 characters.")]
        public string HostelName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{6,}$",
            ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, and 1 special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Document number is required.")]
        [RegularExpression(@"^\d{6,50}$", ErrorMessage = "Document number must be numeric.")]
        public string DocumentNumber { get; set; }

        [Required(ErrorMessage = "Hostel image is required.")]
        public IFormFile HostelImage { get; set; }

        [Required(ErrorMessage = "Document image is required.")]
        public IFormFile DocumentImage { get; set; }

        [Required(ErrorMessage = "Number of room types is required.")]
        [Range(1, 4, ErrorMessage = "Number of room types must be between 1 and 4.")]
        public int NumberOfRoomTypes { get; set; }

        [StringLength(50)]
        public string? RoomType1 { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Rate must be non-negative.")]
        public decimal? RateType1 { get; set; }

        [StringLength(50)]
        public string? RoomType2 { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? RateType2 { get; set; }

        [StringLength(50)]
        public string? RoomType3 { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? RateType3 { get; set; }

        [StringLength(50)]
        public string? RoomType4 { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? RateType4 { get; set; }
    }
}