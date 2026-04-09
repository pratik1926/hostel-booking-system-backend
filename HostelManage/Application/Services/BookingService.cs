using HostelManage.Models;
using HostelManage.Data;
using Microsoft.EntityFrameworkCore;
using HostelManage.Application.Interfaces;
using HostelManage.Application.DTOs;
using AutoMapper;

namespace HostelManage.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public BookingService(AppDbContext context, IEmailService emailService, IMapper mapper)
        {
            _context = context;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<int> GetBookingCount()
        {
            return await _context.Booking.CountAsync();
        }

        public async Task<BookingResponseDTO> AddBooking(BookingCreateDTO dto)
        {
            if (dto.CheckOut <= dto.CheckIn)
                throw new Exception("Check-Out must be after Check-In.");

            var booking = _mapper.Map<Booking>(dto);
            booking.CreationDate = DateTime.Now;
            booking.Status = 0;

            _context.Booking.Add(booking);  
            await _context.SaveChangesAsync();

            return _mapper.Map<BookingResponseDTO>(dto);


        }

        public async Task<IEnumerable<object>> GetBookingsByUserId(int userId)
        {
            return await _context.Booking
                .Where(b => b.UserID == userId)
                .Join(_context.Hostel,
                      booking => booking.HostelID,
                      hostel => hostel.HostelID,
                      (booking, hostel) => new
                      {
                          booking.BookingID,
                          booking.UserID,
                          booking.RoomType,
                          booking.CheckIn,
                          booking.CheckOut,
                          booking.Status,
                          booking.CreationDate,
                          HostelID = hostel.HostelID,
                          HostelName = hostel.HostelName
                      })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetBookingsByHostelId(int hostelId)
        {
            return await _context.Booking
                .Where(b => b.HostelID == hostelId)
                .Join(_context.User,
                      booking => booking.UserID,
                      user => user.UserID,
                      (booking, user) => new
                      {
                          booking.BookingID,
                          booking.HostelID,
                          booking.UserID,
                          UserName = user.Name,
                          booking.RoomType,
                          booking.CheckIn,
                          booking.CheckOut,
                          booking.Status,
                          booking.CreationDate
                      })
                .ToListAsync();
        }

        public async Task UpdateBookingStatus(int id, int status)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
                throw new Exception("Booking not found");

            if (status < 0 || status > 3)
                throw new Exception("Invalid status value.");

            booking.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBooking(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
                throw new Exception("Booking not found");

            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<Booking> GetBookingById(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
                throw new Exception("Booking not found");

            return booking;
        }

        public async Task<IEnumerable<Booking>> GetAllBookings()
        {
            return await _context.Booking.ToListAsync();
        }

        public async Task<object> GetMyProfile(int userId)
        {
            var user = await _context.User
                .Where(u => u.UserID == userId)
                .Select(u => new { u.Name, u.Email })
                .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("User not found");

            return user;
        }

        public async Task<string> BookRoom(int bookingId)
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(b => b.BookingID == bookingId);
            if (booking == null)
                throw new Exception("Booking not found");

            int roomType = booking.RoomType.ToLower() switch
            {
                "single seater" or "one seater" => 1,
                "double seater" or "two seater" => 2,
                "three seater" => 3,
                "four seater" => 4,
                _ => -1
            };

            if (roomType == -1)
                throw new Exception("Invalid RoomType");

            var hostelDesc = await _context.HostelDescription.FirstOrDefaultAsync(h => h.HostelID == booking.HostelID);
            var hostelOwner = await _context.Hostel.FirstOrDefaultAsync(h => h.HostelID == booking.HostelID);
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserID == booking.UserID);

            if (hostelDesc == null || hostelOwner == null || user == null)
                throw new Exception("Required data not found");

            switch (roomType)
            {
                case 1:
                    if (hostelDesc.RoomType1Count <= 0) throw new Exception("No rooms available");
                    hostelDesc.RoomType1Count--; break;
                case 2:
                    if (hostelDesc.RoomType2Count <= 0) throw new Exception("No rooms available");
                    hostelDesc.RoomType2Count--; break;
                case 3:
                    if (hostelDesc.RoomType3Count <= 0) throw new Exception("No rooms available");
                    hostelDesc.RoomType3Count--; break;
                case 4:
                    if (hostelDesc.RoomType4Count <= 0) throw new Exception("No rooms available");
                    hostelDesc.RoomType4Count--; break;
            }

            booking.Status = 3;
            await _context.SaveChangesAsync();

            var userMessage = $"Dear {user.Name}, your booking is confirmed.";
            var ownerMessage = $"New booking by {user.Name}";

            await _emailService.SendEmailAsync(user.Email, "Booking Confirmed", userMessage);
            await _emailService.SendEmailAsync(hostelOwner.Email, "New Booking", ownerMessage);

            return "Booking approved and emails sent.";
        }
    }
}