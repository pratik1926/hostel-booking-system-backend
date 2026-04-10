//using Microsoft.AspNetCore.Mvc;
//using HostelManage.Models;
//using Microsoft.EntityFrameworkCore;
//using HostelManage.Data;
//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;
//using HostelManage.Services;

//namespace HostelManage.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class BookingController : ControllerBase
//    {
//        private readonly AppDbContext _context;
//        private EmailService _emailService;
//        private EmailService emailService;

//        public BookingController(AppDbContext context, EmailService emailService)
//        {
//            _context = context;
//            _emailService = emailService;
//        }

//        // 1. GET: api/Booking/count
//        [HttpGet("count")]
//        public async Task<ActionResult<int>> GetBookingCount()
//        {
//            return await _context.Booking.CountAsync();
//        }

//        // 2. POST: api/Booking
//        [HttpPost]
//        public async Task<ActionResult<Booking>> AddBooking(Booking booking)
//        {
//            // Optional: Validate that CheckOut is later than CheckIn
//            if (booking.CheckOut <= booking.CheckIn)
//            {
//                return BadRequest("Check-Out must be after Check-In.");
//            }

//            booking.CreationDate = DateTime.Now;
//            _context.Booking.Add(booking);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingID }, booking);
//        }

//        // 3. GET: api/Booking/user/{userId}
//        [HttpGet("user/{userId}")]
//        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByUserId(int userId)
//        {
//            var bookings = await _context.Booking
//                .Where(b => b.UserID == userId)
//                .Join(_context.Hostel,
//                      booking => booking.HostelID,
//                      hostel => hostel.HostelID,
//                      (booking, hostel) => new
//                      {
//                          booking.BookingID,
//                          booking.UserID,
//                          booking.RoomType,
//                          booking.CheckIn,
//                          booking.CheckOut,
//                          booking.Status,
//                          booking.CreationDate,
//                          HostelID = hostel.HostelID,
//                          HostelName = hostel.HostelName  // 👈 Add this
//                      })
//                .ToListAsync();

//            return Ok(bookings);
//        }

//        // 4. GET: api/Booking/hostel/{hostelId}
//        [HttpGet("hostel/{hostelId}")]
//        public async Task<IActionResult> GetBookingsByHostelId(int hostelId)
//        {
//            var bookingsWithUser = await _context.Booking
//                .Where(b => b.HostelID == hostelId)
//                .Join(_context.User,
//                      booking => booking.UserID,
//                      user => user.UserID,
//                      (booking, user) => new
//                      {
//                          booking.BookingID,
//                          booking.HostelID,
//                          booking.UserID,
//                          UserName = user.Name, // 👈 Username from User table
//                          booking.RoomType,
//                          booking.CheckIn,
//                          booking.CheckOut,
//                          booking.Status,
//                          booking.CreationDate
//                      })
//                .ToListAsync();

//            return Ok(bookingsWithUser);
//        }

//        // 5. PUT: api/Booking/status/{id}? status = 1
//        [HttpPut("status/{id}")]
//        public async Task<IActionResult> UpdateBookingStatus(int id, [FromQuery] int status)
//        {
//            var booking = await _context.Booking.FindAsync(id);
//            if (booking == null)
//                return NotFound();

//            if (status < 0 || status > 3)
//                return BadRequest("Invalid status value. Use 0 = Pending, 1 = Confirmed, 2 = Cancelled, 3 = Approved");

//            booking.Status = status;
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }



//        // 6. DELETE: api/Booking/{id}
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteBooking(int id)
//        {
//            var booking = await _context.Booking.FindAsync(id);
//            if (booking == null)
//                return NotFound();

//            _context.Booking.Remove(booking);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        // Helper: GET Booking by ID
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Booking>> GetBookingById(int id)
//        {
//            var booking = await _context.Booking.FindAsync(id);
//            if (booking == null)
//                return NotFound();

//            return booking;
//        }

//        //GET: api/Booking For admin
//       [HttpGet]
//        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
//        {
//            return await _context.Booking.ToListAsync();
//        }

//        [Authorize]
//        [HttpGet("me")]
//        public async Task<IActionResult> GetMyProfile()
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
//            if (userIdClaim == null)
//                return Unauthorized("User ID not found in token.");

//            int userId = int.Parse(userIdClaim.Value);

//            var user = await _context.User
//                .Where(u => u.UserID == userId)
//                .Select(u => new
//                {
//                    u.Name,
//                    u.Email
//                })
//                .FirstOrDefaultAsync();

//            if (user == null)
//                return NotFound("User not found.");

//            return Ok(user);
//        }

//        //[HttpPut("book-room/{bookingId}")]
//        //public async Task<IActionResult> BookRoom(int bookingId)
//        //{
//        //    // Get the booking
//        //    var booking = await _context.Booking.FirstOrDefaultAsync(b => b.BookingID == bookingId);
//        //    if (booking == null)
//        //        return NotFound("Booking not found");

//        //    // Map string RoomType to int
//        //    int roomType;
//        //    switch (booking.RoomType.ToLower())
//        //    {
//        //        case "single seater":
//        //        case "one seater":
//        //            roomType = 1;
//        //            break;
//        //        case "double seater":
//        //        case "two seater":
//        //            roomType = 2;
//        //            break;
//        //        case "three seater":

//        //            roomType = 3;
//        //            break;
//        //        case "four seater":
//        //            roomType = 4;
//        //            break;
//        //        default:
//        //            return BadRequest("Invalid RoomType value in booking");
//        //    }

//        //    // Get hostel description entry
//        //    var hostel = await _context.HostelDescription.FirstOrDefaultAsync(h => h.HostelID == booking.HostelID);
//        //    if (hostel == null)
//        //        return NotFound("Hostel not found");

//        //    // Reduce the appropriate room type count
//        //    switch (roomType)
//        //    {
//        //        case 1:
//        //            if (hostel.RoomType1Count <= 0) return BadRequest("No rooms of type 1 available");
//        //            hostel.RoomType1Count -= 1;
//        //            break;
//        //        case 2:
//        //            if (hostel.RoomType2Count <= 0) return BadRequest("No rooms of type 2 available");
//        //            hostel.RoomType2Count -= 1;
//        //            break;
//        //        case 3:
//        //            if (hostel.RoomType3Count <= 0) return BadRequest("No rooms of type 3 available");
//        //            hostel.RoomType3Count -= 1;
//        //            break;
//        //        case 4:
//        //            if (hostel.RoomType4Count <= 0) return BadRequest("No rooms of type 4 available");
//        //            hostel.RoomType4Count -= 1;
//        //            break;
//        //    }

//        //    // Set status to Approved (3)
//        //    booking.Status = 3;

//        //    await _context.SaveChangesAsync();

//        //    return Ok(new
//        //    {
//        //        message = $"Booking approved and room count updated for RoomType: {booking.RoomType}"
//        //    });
//        //}

//        [HttpPut("book-room/{bookingId}")]
//        public async Task<IActionResult> BookRoom(int bookingId)
//        {
//            var booking = await _context.Booking.FirstOrDefaultAsync(b => b.BookingID == bookingId);
//            if (booking == null)
//                return NotFound("Booking not found");

//            int roomType;
//            switch (booking.RoomType.ToLower())
//            {
//                case "single seater":
//                case "one seater":
//                    roomType = 1; break;
//                case "double seater":
//                case "two seater":
//                    roomType = 2; break;
//                case "three seater":
//                    roomType = 3; break;
//                case "four seater":
//                    roomType = 4; break;
//                default:
//                    return BadRequest("Invalid RoomType value in booking");
//            }

//            var hostelDesc = await _context.HostelDescription.FirstOrDefaultAsync(h => h.HostelID == booking.HostelID);
//            if (hostelDesc == null)
//                return NotFound("Hostel not found");

//            var hostelOwner = await _context.Hostel.FirstOrDefaultAsync(h => h.HostelID == booking.HostelID);
//            if (hostelOwner == null)
//                return NotFound("Hostel owner not found");

//            var user = await _context.User.FirstOrDefaultAsync(u => u.UserID == booking.UserID);
//            if (user == null)
//                return NotFound("User not found");

//            switch (roomType)
//            {
//                case 1:
//                    if (hostelDesc.RoomType1Count <= 0) return BadRequest("No rooms of type 1 available");
//                    hostelDesc.RoomType1Count--; break;
//                case 2:
//                    if (hostelDesc.RoomType2Count <= 0) return BadRequest("No rooms of type 2 available");
//                    hostelDesc.RoomType2Count--; break;
//                case 3:
//                    if (hostelDesc.RoomType3Count <= 0) return BadRequest("No rooms of type 3 available");
//                    hostelDesc.RoomType3Count--; break;
//                case 4:
//                    if (hostelDesc.RoomType4Count <= 0) return BadRequest("No rooms of type 4 available");
//                    hostelDesc.RoomType4Count--; break;
//            }

//            booking.Status = 3;
//            await _context.SaveChangesAsync();

//            var userMessage = $"Dear {user.Name},\n\nYour booking for a {booking.RoomType} in {hostelOwner.HostelName} has been confirmed.\nCheck-In: {booking.CheckIn:dd MMM yyyy}\nCheck-Out: {booking.CheckOut:dd MMM yyyy}\n\nThank you!";
//            var ownerMessage = $"Dear {hostelOwner.HostelName},\n\nA new booking has been confirmed.\nUser: {user.Name}\nRoom Type: {booking.RoomType}\nCheck-In: {booking.CheckIn:dd MMM yyyy}\nCheck-Out: {booking.CheckOut:dd MMM yyyy}";

//            // ✅ Send emails
//            await _emailService.SendEmailAsync(user.Email, "Booking Confirmed", userMessage);
//            await _emailService.SendEmailAsync(hostelOwner.Email, "New Booking Received", ownerMessage);

//            return Ok(new
//            {
//                message = $"Booking approved and emails sent to {user.Email} and {hostelOwner.Email}."
//            });
//        }

//    }
//}

//using Microsoft.AspNetCore.Mvc;
//using HostelManage.Models;
//using HostelManage.Application.Interfaces;
//namespace HostelManage.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class BookingController : ControllerBase
//    {
//        private readonly IBookingService _service;

//        public BookingController(IBookingService service)
//        {
//            _service = service;
//        }

//        [HttpGet("count")]
//        public async Task<ActionResult<int>> GetBookingCount()
//        {
//            return await _service.GetBookingCount();
//        }

//        [HttpPost]
//        public async Task<ActionResult<Booking>> AddBooking(Booking booking)
//        {
//            try
//            {
//                var result = await _service.CreateBooking(booking);

//                return CreatedAtAction(nameof(GetBookingById), new { id = result.BookingID }, result);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpGet("user/{userId}")]
//        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByUserId(int userId)
//        {
//            var bookings = await _context.Booking
//                .Where(b => b.UserID == userId)
//                .Join(_context.Hostel,
//                      booking => booking.HostelID,
//                      hostel => hostel.HostelID,
//                      (booking, hostel) => new
//                      {
//                          booking.BookingID,
//                          booking.UserID,
//                          booking.RoomType,
//                          booking.CheckIn,
//                          booking.CheckOut,
//                          booking.Status,
//                          booking.CreationDate,
//                          HostelID = hostel.HostelID,
//                          HostelName = hostel.HostelName
//                      })
//                .ToListAsync();

//            return Ok(bookings);
//        }

//        [HttpGet("hostel/{hostelId}")]
//        public async Task<IActionResult> GetBookingsByHostelId(int hostelId)
//        {
//            var bookingsWithUser = await _context.Booking
//                .Where(b => b.HostelID == hostelId)
//                .Join(_context.User,
//                      booking => booking.UserID,
//                      user => user.UserID,
//                      (booking, user) => new
//                      {
//                          booking.BookingID,
//                          booking.HostelID,
//                          booking.UserID,
//                          UserName = user.Name,
//                          booking.RoomType,
//                          booking.CheckIn,
//                          booking.CheckOut,
//                          booking.Status,
//                          booking.CreationDate
//                      })
//                .ToListAsync();

//            return Ok(bookingsWithUser);
//        }

//        [HttpPut("status/{id}")]
//        public async Task<IActionResult> UpdateBookingStatus(int id, [FromQuery] int status)
//        {
//            var booking = await _context.Booking.FindAsync(id);
//            if (booking == null)
//                return NotFound();

//            if (status < 0 || status > 3)
//                return BadRequest("Invalid status value. Use 0 = Pending, 1 = Confirmed, 2 = Cancelled, 3 = Approved");

//            booking.Status = status;
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteBooking(int id)
//        {
//            var booking = await _context.Booking.FindAsync(id);
//            if (booking == null)
//                return NotFound();

//            _context.Booking.Remove(booking);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<Booking>> GetBookingById(int id)
//        {
//            var booking = await _context.Booking.FindAsync(id);
//            if (booking == null)
//                return NotFound();

//            return booking;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
//        {
//            return await _context.Booking.ToListAsync();
//        }

//        [Authorize]
//        [HttpGet("me")]
//        public async Task<IActionResult> GetMyProfile()
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
//            if (userIdClaim == null)
//                return Unauthorized("User ID not found in token.");

//            int userId = int.Parse(userIdClaim.Value);

//            var user = await _context.User
//                .Where(u => u.UserID == userId)
//                .Select(u => new { u.Name, u.Email })
//                .FirstOrDefaultAsync();

//            if (user == null)
//                return NotFound("User not found.");

//            return Ok(user);
//        }

//        [HttpPut("book-room/{bookingId}")]
//        public async Task<IActionResult> BookRoom(int bookingId)
//        {
//            var booking = await _context.Booking.FirstOrDefaultAsync(b => b.BookingID == bookingId);
//            if (booking == null)
//                return NotFound("Booking not found");

//            int roomType = booking.RoomType.ToLower() switch
//            {
//                "single seater" or "one seater" => 1,
//                "double seater" or "two seater" => 2,
//                "three seater" => 3,
//                "four seater" => 4,
//                _ => -1
//            };

//            if (roomType == -1)
//                return BadRequest("Invalid RoomType value in booking");

//            var hostelDesc = await _context.HostelDescription.FirstOrDefaultAsync(h => h.HostelID == booking.HostelID);
//            if (hostelDesc == null)
//                return NotFound("Hostel not found");

//            var hostelOwner = await _context.Hostel.FirstOrDefaultAsync(h => h.HostelID == booking.HostelID);
//            if (hostelOwner == null)
//                return NotFound("Hostel owner not found");

//            var user = await _context.User.FirstOrDefaultAsync(u => u.UserID == booking.UserID);
//            if (user == null)
//                return NotFound("User not found");

//            switch (roomType)
//            {
//                case 1:
//                    if (hostelDesc.RoomType1Count <= 0) return BadRequest("No rooms of type 1 available");
//                    hostelDesc.RoomType1Count--; break;
//                case 2:
//                    if (hostelDesc.RoomType2Count <= 0) return BadRequest("No rooms of type 2 available");
//                    hostelDesc.RoomType2Count--; break;
//                case 3:
//                    if (hostelDesc.RoomType3Count <= 0) return BadRequest("No rooms of type 3 available");
//                    hostelDesc.RoomType3Count--; break;
//                case 4:
//                    if (hostelDesc.RoomType4Count <= 0) return BadRequest("No rooms of type 4 available");
//                    hostelDesc.RoomType4Count--; break;
//            }

//            booking.Status = 3;
//            await _context.SaveChangesAsync();

//            var userMessage = $"Dear {user.Name},\n\nYour booking for a {booking.RoomType} in {hostelOwner.HostelName} has been confirmed.\nCheck-In: {booking.CheckIn:dd MMM yyyy}\nCheck-Out: {booking.CheckOut:dd MMM yyyy}\n\nThank you!";
//            var ownerMessage = $"Dear {hostelOwner.HostelName},\n\nA new booking has been confirmed.\nUser: {user.Name}\nRoom Type: {booking.RoomType}\nCheck-In: {booking.CheckIn:dd MMM yyyy}\nCheck-Out: {booking.CheckOut:dd MMM yyyy}";

//            await _emailService.SendEmailAsync(user.Email, "Booking Confirmed", userMessage);
//            await _emailService.SendEmailAsync(hostelOwner.Email, "New Booking Received", ownerMessage);

//            return Ok(new
//            {
//                message = $"Booking approved and emails sent to {user.Email} and {hostelOwner.Email}."
//            });
//        }
//    }
//}

using HostelManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HostelManage.Application.DTOs.Booking;

namespace HostelManage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetBookingCount()
        {
            return await _service.GetBookingCount();
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(BookingCreateDTO bookingDto)
        {
            try
            {
                var result = await _service.AddBooking(bookingDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUserId(int userId)
        {
            return Ok(await _service.GetBookingsByUserId(userId));
        }

        [HttpGet("hostel/{hostelId}")]
        public async Task<IActionResult> GetBookingsByHostelId(int hostelId)
        {
            return Ok(await _service.GetBookingsByHostelId(hostelId));
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromQuery] int status)
        {
            try
            {
                await _service.UpdateBookingStatus(id, status);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                await _service.DeleteBooking(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBookingById(int id)
        {
            try
            {
                return await _service.GetBookingById(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            return Ok(await _service.GetAllBookings());
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            try
            {
                return Ok(await _service.GetMyProfile(userId));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("book-room/{bookingId}")]
        public async Task<IActionResult> BookRoom(int bookingId)
        {
            try
            {
                var result = await _service.BookRoom(bookingId);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
