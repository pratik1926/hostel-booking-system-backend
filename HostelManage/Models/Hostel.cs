using System;
using System.ComponentModel.DataAnnotations;

namespace HostelManage.Models
{
    public class Hostel
    {
        internal int RoomType1Count;
        internal int RoomType2Count;
        internal int RoomType3Count;

        [Key]
        public int HostelID { get; set; }

        [Required(ErrorMessage = "Hostel name is required.")]
        [StringLength(100, ErrorMessage = "Hostel name cannot exceed 100 characters.")]
        public string HostelName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Number of room types is required.")]
        [Range(1, 4, ErrorMessage = "Number of room types must be between 1 and 4.")]
        public int NumberOfRoomTypes { get; set; }

        [StringLength(50, ErrorMessage = "Room type name cannot exceed 50 characters.")]
        public string? RoomType1 { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Rate must be non-negative.")]
        public decimal? RateType1 { get; set; }

        [StringLength(50, ErrorMessage = "Room type name cannot exceed 50 characters.")]
        public string? RoomType2 { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Rate must be non-negative.")]
        public decimal? RateType2 { get; set; }

        [StringLength(50, ErrorMessage = "Room type name cannot exceed 50 characters.")]
        public string? RoomType3 { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Rate must be non-negative.")]
        public decimal? RateType3 { get; set; }

        [StringLength(50, ErrorMessage = "Room type name cannot exceed 50 characters.")]
        public string? RoomType4 { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Rate must be non-negative.")]
        public decimal? RateType4 { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        public byte[]? DocumentImage { get; set; }

        [Required]
        [MaxLength(50)]
        public string DocumentNumber { get; set; }


        public byte[]? HostelImage { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public bool Status { get; set; } = false; // Default: unverified

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
