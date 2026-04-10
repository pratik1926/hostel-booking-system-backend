using HostelManage.Data;
using HostelManage.Models;
using HostelManage.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HostelManage.Application.DTOs.Hostel;

namespace HostelManage.Application.Services
{
    public class HostelService : IHostelService
    {
        private readonly AppDbContext _context;

        public HostelService(AppDbContext context)
        {
            _context = context;
        }

        // CREATE HOSTEL
        public async Task<string> CreateHostel(HostelCreateDTO request)
        {
            var existingHostel = await _context.Hostel
                .FirstOrDefaultAsync(h => h.Email == request.Email.ToLower());

            if (existingHostel != null)
                throw new Exception("Email is already registered.");

            byte[] hostelImageBytes = await ConvertToBytes(request.HostelImage);
            byte[] documentImageBytes = await ConvertToBytes(request.DocumentImage);

            var hostel = new Hostel
            {
                HostelName = request.HostelName,
                Address = request.Address,
                Email = request.Email.ToLower(),
                DocumentNumber = request.DocumentNumber,
                DocumentImage = documentImageBytes,
                HostelImage = hostelImageBytes,
                Status = false,
                CreationDate = DateTime.Now,
                NumberOfRoomTypes = request.NumberOfRoomTypes,
                RoomType1 = request.RoomType1,
                RateType1 = request.RateType1,
                RoomType2 = request.RoomType2,
                RateType2 = request.RateType2,
                RoomType3 = request.RoomType3,
                RateType3 = request.RateType3,
                RoomType4 = request.RoomType4,
                RateType4 = request.RateType4
            };

            var passwordHasher = new PasswordHasher<Hostel>();
            hostel.Password = passwordHasher.HashPassword(hostel, request.Password);

            _context.Hostel.Add(hostel);
            await _context.SaveChangesAsync();

            return "Hostel created successfully.";
        }

        // LOGIN
        public async Task<object> Login(LoginDTO request)
        {
            var hostel = await _context.Hostel
                .FirstOrDefaultAsync(h => h.Email == request.Email.ToLower());

            if (hostel == null)
                throw new Exception("Hostel not found.");

            var hasher = new PasswordHasher<Hostel>();
            var result = hasher.VerifyHashedPassword(hostel, hostel.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid password.");

            return new
            {
                hostel.HostelID,
                hostel.HostelName
            };
        }

        // RESET PASSWORD
        public async Task<string> ResetPassword(HostelResetPasswordDTO request)
        {
            var hostel = await _context.Hostel
                .FirstOrDefaultAsync(h => h.Email == request.Email.ToLower());

            if (hostel == null)
                throw new Exception("Hostel not found.");

            var hasher = new PasswordHasher<Hostel>();
            hostel.Password = hasher.HashPassword(hostel, request.NewPassword);

            await _context.SaveChangesAsync();

            return "Password reset successfully.";
        }

        // EDIT PROFILE
        public async Task<string> EditProfile(int hostelId, HostelEditDTO request)
        {
            var hostel = await _context.Hostel.FindAsync(hostelId);

            if (hostel == null)
                throw new Exception("Hostel not found.");

            if (!string.IsNullOrEmpty(request.HostelName))
                hostel.HostelName = request.HostelName;

            if (!string.IsNullOrEmpty(request.Address))
                hostel.Address = request.Address;

            hostel.NumberOfRoomTypes = request.NumberOfRoomTypes;
            hostel.RoomType1 = request.RoomType1;
            hostel.RateType1 = request.RateType1;
            hostel.RoomType2 = request.RoomType2;
            hostel.RateType2 = request.RateType2;
            hostel.RoomType3 = request.RoomType3;
            hostel.RateType3 = request.RateType3;
            hostel.RoomType4 = request.RoomType4;
            hostel.RateType4 = request.RateType4;

            if (request.HostelImage != null)
                hostel.HostelImage = await ConvertToBytes(request.HostelImage);

            await _context.SaveChangesAsync();

            return "Hostel profile updated successfully.";
        }

        // GET BY ID
        public async Task<object> GetHostelById(int id)
        {
            var hostel = await _context.Hostel.FindAsync(id);

            if (hostel == null)
                throw new Exception("Hostel not found.");

            return new
            {
                hostel.HostelID,
                hostel.HostelName,
                hostel.Address,
                hostel.Email,
                hostel.Status,
                hostel.CreationDate,
                hostel.NumberOfRoomTypes,
                hostel.RoomType1,
                hostel.RateType1,
                hostel.RoomType2,
                hostel.RateType2,
                hostel.RoomType3,
                hostel.RateType3,
                hostel.RoomType4,
                hostel.RateType4,
                HostelImageBase64 = hostel.HostelImage != null ? Convert.ToBase64String(hostel.HostelImage) : null,
                DocumentImageBase64 = hostel.DocumentImage != null ? Convert.ToBase64String(hostel.DocumentImage) : null
            };
        }

        // HELPER METHOD
        private async Task<byte[]> ConvertToBytes(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}