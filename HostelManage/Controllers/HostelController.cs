//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using HostelManage.Models;
//using HostelManage.Data;
//using System.Threading.Tasks;
//using System.ComponentModel.DataAnnotations;
//using Microsoft.AspNetCore.Identity.Data;
//using Microsoft.AspNetCore.Identity;

//namespace HostelManage.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class HostelController : ControllerBase
//    {
//        private readonly AppDbContext _context;
//        private readonly ILogger<HostelController> _logger;

//        public HostelController(AppDbContext context, ILogger<HostelController> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        // POST: api/hostel/create
//        [HttpPost("create")]
//        public async Task<IActionResult> CreateHostel([FromForm] HostelRequest request)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);
//            // Required field validations
//            if (string.IsNullOrEmpty(request.HostelName) ||
//                string.IsNullOrEmpty(request.Address) ||
//                string.IsNullOrEmpty(request.Email) ||
//                string.IsNullOrWhiteSpace(request.Password) ||
//                string.IsNullOrEmpty(request.DocumentNumber) || !long.TryParse(request.DocumentNumber, out var parsedDocNum) || parsedDocNum <= 0)

//            {
//                _logger.LogWarning("Required fields are missing.");
//                return BadRequest(new { message = "All required fields must be provided." });
//            }

//            // Check if email already exists
//            var existingHostel = await _context.Hostel
//                .FirstOrDefaultAsync(h => h.Email == request.Email.ToLower());
//            if (existingHostel != null)
//            {
//                _logger.LogWarning($"Email {request.Email} is already registered.");
//                return BadRequest(new { message = "Email is already registered." });
//            }

//            // Check for image files
//            if (request.HostelImage == null || request.DocumentImage == null)
//            {
//                _logger.LogWarning("Hostel image or DocumentImage is missing.");
//                return BadRequest(new { message = "Hostel Image and Document Image are required." });
//            }

//            // Converting Images to Byte Arrays
//            byte[] hostelImageBytes;
//            byte[] documentImageBytes;

//            using (var memoryStream = new MemoryStream())
//            {
//                await request.HostelImage.CopyToAsync(memoryStream);
//                hostelImageBytes = memoryStream.ToArray();
//            }

//            using (var memoryStream = new MemoryStream())
//            {
//                await request.DocumentImage.CopyToAsync(memoryStream);
//                documentImageBytes = memoryStream.ToArray();
//            }
//            var passwordHasher = new PasswordHasher<Hostel>();
//            // Creating Hostel Object
//            var hostel = new Hostel
//            {
//                HostelName = request.HostelName,
//                Address = request.Address,
//                Email = request.Email.ToLower(),
//                DocumentNumber = request.DocumentNumber,
//                DocumentImage = documentImageBytes,
//                HostelImage = hostelImageBytes,
//                Status = false, // Default status as false (Unverified)
//                CreationDate = DateTime.Now, // Set creation date to current time
//                NumberOfRoomTypes = request.NumberOfRoomTypes,
//                RoomType1 = request.RoomType1,
//                RateType1 = request.RateType1,
//                RoomType2 = request.RoomType2,
//                RateType2 = request.RateType2,
//                RoomType3 = request.RoomType3,
//                RateType3 = request.RateType3,
//                RoomType4 = request.RoomType4,
//                RateType4 = request.RateType4
//            };
//            hostel.Password = passwordHasher.HashPassword(hostel, request.Password);

//            try
//            {
//                _context.Hostel.Add(hostel);
//                await _context.SaveChangesAsync();
//                return Ok(new { message = "Hostel created successfully." });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error saving hostel: {ex.Message}");
//                return StatusCode(500, new { message = "An error occurred while saving the hostel." });
//            }
//        }

//        [HttpPost("reset-password")]
//        public async Task<IActionResult> ResetPassword([FromBody] HostelResetPasswordRequest request)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            if (string.IsNullOrWhiteSpace(request.NewPassword))
//                return BadRequest(new { message = "Password cannot be empty or just whitespace." });

//            var hostel = await _context.Hostel
//                .FirstOrDefaultAsync(h => h.Email == request.Email.ToLower());
//            if (hostel == null)
//            {
//                _logger.LogWarning("Reset-password: hostel with email {Email} not found.", request.Email);
//                return NotFound(new { message = "Hostel not found." });
//            }

//            var hasher = new PasswordHasher<Hostel>();
//            hostel.Password = hasher.HashPassword(hostel, request.NewPassword.Trim());

//            try
//            {
//                _context.Entry(hostel).State = EntityState.Modified;
//                await _context.SaveChangesAsync();
//                return Ok(new { message = "Password has been reset successfully." });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error resetting password for {Email}.", request.Email);
//                return StatusCode(500, new { message = "An error occurred while resetting the password." });
//            }
//        }

//        // POST: api/hostel/login
//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginRequest request)
//        {
//            // 1. Validate required fields
//            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
//            {
//                _logger.LogWarning("Email or Password is missing.");
//                return BadRequest(new { message = "Email and Password are required." });
//            }

//            // 2. Lookup hostel by email
//            var hostel = await _context.Hostel
//                .FirstOrDefaultAsync(h => h.Email == request.Email.ToLower());
//            if (hostel == null)
//            {
//                _logger.LogWarning($"Hostel with email {request.Email} not found.");
//                return NotFound(new { message = "Hostel not found." });
//            }

//            // 3. Verify the hashed password
//            var hasher = new PasswordHasher<Hostel>();
//            var result = hasher.VerifyHashedPassword(hostel, hostel.Password, request.Password);
//            if (result == PasswordVerificationResult.Failed)
//            {
//                _logger.LogWarning("Invalid password for email {Email}.", request.Email);
//                return Unauthorized(new { message = "Invalid password." });
//            }

//            // 4. Success — return minimal hostel info
//            return Ok(new
//            {
//                HostelID = hostel.HostelID,
//                HostelName = hostel.HostelName
//            });
//        }

//        [HttpPut("edit-profile/{hostelId}")]
//        public async Task<IActionResult> EditHostelProfile(int hostelId, [FromForm] HostelEditRequest request)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);
//            var hostel = await _context.Hostel.FindAsync(hostelId);
//            if (hostel == null)
//            {
//                _logger.LogWarning("EditProfile: Hostel with ID {HostelId} not found.", hostelId);
//                return NotFound(new { message = "Hostel not found." });
//            }

//            // Update editable fields
//            if (!string.IsNullOrEmpty(request.HostelName))
//                hostel.HostelName = request.HostelName;

//            if (!string.IsNullOrEmpty(request.Address))
//                hostel.Address = request.Address;

//            hostel.NumberOfRoomTypes = request.NumberOfRoomTypes;
//            hostel.RoomType1 = request.RoomType1;
//            hostel.RateType1 = request.RateType1;
//            hostel.RoomType2 = request.RoomType2;
//            hostel.RateType2 = request.RateType2;
//            hostel.RoomType3 = request.RoomType3;
//            hostel.RateType3 = request.RateType3;
//            hostel.RoomType4 = request.RoomType4;
//            hostel.RateType4 = request.RateType4;

//            // Optional image update
//            if (request.HostelImage != null)
//            {
//                using (var memoryStream = new MemoryStream())
//                {
//                    await request.HostelImage.CopyToAsync(memoryStream);
//                    hostel.HostelImage = memoryStream.ToArray();
//                }
//            }

//            try
//            {
//                _context.Entry(hostel).State = EntityState.Modified;
//                await _context.SaveChangesAsync();
//                return Ok(new { message = "Hostel profile updated successfully." });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating profile for HostelID {HostelId}.", hostelId);
//                return StatusCode(500, new { message = "An error occurred while updating the hostel profile." });
//            }
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetHostelById(int id)
//        {
//            var hostel = await _context.Hostel.FindAsync(id);

//            if (hostel == null)
//            {
//                _logger.LogWarning("Hostel with ID {HostelId} not found.", id);
//                return NotFound(new { message = "Hostel not found." });
//            }

//            // Return essential fields only (avoid returning password or large byte arrays directly)
//            return Ok(new
//            {
//                hostel.HostelID,
//                hostel.HostelName,
//                hostel.Address,
//                hostel.Email,
//                hostel.Status,
//                hostel.CreationDate,
//                hostel.NumberOfRoomTypes,
//                hostel.RoomType1,
//                hostel.RateType1,
//                hostel.RoomType2,
//                hostel.RateType2,
//                hostel.RoomType3,
//                hostel.RateType3,
//                hostel.RoomType4,
//                hostel.RateType4,
//                // Optional: convert image to base64 if you need to show them
//                HostelImageBase64 = hostel.HostelImage != null ? Convert.ToBase64String(hostel.HostelImage) : null,
//                DocumentImageBase64 = hostel.DocumentImage != null ? Convert.ToBase64String(hostel.DocumentImage) : null
//            });
//        }




//    }
//    public class HostelRequest
//    {
//        [Required(ErrorMessage = "Hostel name is required.")]
//        [StringLength(100)]
//        public string HostelName { get; set; }

//        [Required(ErrorMessage = "Address is required.")]
//        [StringLength(255)]
//        public string Address { get; set; }

//        [Required(ErrorMessage = "Email is required.")]
//        [EmailAddress]
//        [StringLength(100)]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "Password is required.")]
//        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
//        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{6,}$",
//    ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, and 1 special character.")]
//        public string Password { get; set; }

//        [Required(ErrorMessage = "Document number is required.")]
//        [RegularExpression(@"^\d{6,50}$", ErrorMessage = "Document number must be numeric.")]
//        public string DocumentNumber { get; set; }

//        [Required(ErrorMessage = "Hostel image is required.")]
//        public IFormFile HostelImage { get; set; }

//        [Required(ErrorMessage = "Document image is required.")]
//        public IFormFile DocumentImage { get; set; }

//        [Required]
//        [Range(1, 4, ErrorMessage = "Number of room types must be between 1 and 4.")]
//        public int NumberOfRoomTypes { get; set; }

//        [StringLength(50)]
//        public string? RoomType1 { get; set; }

//        [Range(0, double.MaxValue, ErrorMessage = "Rate must be non-negative.")]
//        public decimal? RateType1 { get; set; }

//        [StringLength(50)]
//        public string? RoomType2 { get; set; }

//        [Range(0, double.MaxValue)]
//        public decimal? RateType2 { get; set; }

//        [StringLength(50)]
//        public string? RoomType3 { get; set; }

//        [Range(0, double.MaxValue)]
//        public decimal? RateType3 { get; set; }

//        [StringLength(50)]
//        public string? RoomType4 { get; set; }

//        [Range(0, double.MaxValue)]
//        public decimal? RateType4 { get; set; }
//    }
//    public class LoginRequest
//    {
//        [Required]
//        [EmailAddress]
//        [StringLength(100)]
//        public string Email { get; set; }

//        [Required]
//        [StringLength(255)]
//        public string Password { get; set; }
//    }

//    public class HostelResetPasswordRequest
//    {
//        [Required, EmailAddress, StringLength(100)]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "Password is required.")]
//        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
//        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{6,}$",
//            ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, and 1 special character.")]
//        public string NewPassword { get; set; }
//    }

//    public class HostelEditRequest
//    {
//        [StringLength(100)]
//        public string HostelName { get; set; }

//        [StringLength(255)]
//        public string Address { get; set; }

//        public IFormFile HostelImage { get; set; }

//        [Range(1, 4)]
//        public int NumberOfRoomTypes { get; set; }

//        public string? RoomType1 { get; set; }
//        public decimal? RateType1 { get; set; }

//        public string? RoomType2 { get; set; }
//        public decimal? RateType2 { get; set; }

//        public string? RoomType3 { get; set; }
//        public decimal? RateType3 { get; set; }

//        public string? RoomType4 { get; set; }
//        public decimal? RateType4 { get; set; }
//    }


//}


using Microsoft.AspNetCore.Mvc;
using HostelManage.Application.Interfaces;
using HostelManage.Application.DTOs;

namespace HostelManage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostelController : ControllerBase
    {
        private readonly IHostelService _service;

        public HostelController(IHostelService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateHostel([FromForm] HostelCreateDTO request)
        {
            try
            {
                var result = await _service.CreateHostel(request);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            try
            {
                var result = await _service.Login(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] HostelResetPasswordDTO request)
        {
            try
            {
                var result = await _service.ResetPassword(request);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit-profile/{id}")]
        public async Task<IActionResult> EditProfile(int id, [FromForm] HostelEditDTO request)
        {
            try
            {
                var result = await _service.EditProfile(id, request);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHostelById(int id)
        {
            try
            {
                var result = await _service.GetHostelById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}