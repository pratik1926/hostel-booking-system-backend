using System.ComponentModel.DataAnnotations;

namespace FYP_Backend.Model
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]  // Ensures only the date part is considered
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string EmailAddress { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        public string PermanentAddress { get; set; }
    }
}
