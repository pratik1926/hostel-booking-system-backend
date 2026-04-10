using HostelManage.Data;
using HostelManage.Models;
using HostelManage.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HostelManage.Application.DTOs.User;

namespace HostelManage.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // CREATE USER
        public async Task<string> CreateUser(UserCreateDTO dto)
        {
            if (dto.Image == null)
                throw new Exception("User image is required");

            var existingUser = await _context.User
                .FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower());

            if (existingUser != null)
                throw new Exception("Email already exists");

            byte[] imageBytes = await ConvertToBytes(dto.Image);

            var user = new User
            {
                Name = dto.Name,
                Address = dto.Address,
                Email = dto.Email.ToLower(),
                Contact = dto.Contact,
                Image = imageBytes
            };

            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, dto.Password);

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return "User created successfully";
        }

        // LOGIN
        public async Task<object> Login(UserLoginDTO dto)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower());

            if (user == null)
                throw new Exception("User not found");

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid password");

            return new
            {
                user.UserID,
                user.Name
            };
        }

        // RESET PASSWORD
        public async Task<string> ResetPassword(UserResetPasswordDTO dto)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower());

            if (user == null)
                throw new Exception("User not found");

            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, dto.NewPassword);

            await _context.SaveChangesAsync();

            return "Password reset successful";
        }

        // GET USER DETAILS
        public async Task<object> GetUserDetails(int userId)
        {
            var user = await _context.User
                .Where(u => u.UserID == userId)
                .Select(u => new
                {
                    u.UserID,
                    u.Name,
                    u.Address,
                    u.Email,
                    Contact = u.Contact ?? "",
                    Image = u.Image != null ? Convert.ToBase64String(u.Image) : null
                })
                .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("User not found");

            return user;
        }

        // EDIT PROFILE
        public async Task<string> EditProfile(int userId, UserEditDTO dto)
        {
            var user = await _context.User.FindAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            if (!string.IsNullOrEmpty(dto.Name))
                user.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Address))
                user.Address = dto.Address;

            if (!string.IsNullOrEmpty(dto.Contact))
                user.Contact = dto.Contact;

            if (dto.Image != null)
                user.Image = await ConvertToBytes(dto.Image);

            await _context.SaveChangesAsync();

            return "Profile updated successfully";
        }

        // GET HOSTELS
        public async Task<object> GetHostels()
        {
            return await _context.Hostel
                .Where(h => h.Status == true)
                .Select(h => new
                {
                    h.HostelID,
                    h.HostelName,
                    h.Address,
                    h.NumberOfRoomTypes,
                    h.RoomType1,
                    h.RateType1,
                    h.RoomType2,
                    h.RateType2,
                    h.RoomType3,
                    h.RateType3,
                    h.RoomType4,
                    h.RateType4,
                    HostelImage = h.HostelImage != null ? Convert.ToBase64String(h.HostelImage) : null
                })
                .ToListAsync();
        }

        // GET HOSTEL ROOMS
        public async Task<object> GetHostelRooms(int hostelId)
        {
            return await _context.HostelRoom
                .Where(r => r.HostelID == hostelId)
                .Select(r => new
                {
                    r.RoomID,
                    RoomImage1 = r.RoomImage1 != null ? Convert.ToBase64String(r.RoomImage1) : null,
                    RoomImage2 = r.RoomImage2 != null ? Convert.ToBase64String(r.RoomImage2) : null,
                    RoomImage3 = r.RoomImage3 != null ? Convert.ToBase64String(r.RoomImage3) : null,
                    RoomImage4 = r.RoomImage4 != null ? Convert.ToBase64String(r.RoomImage4) : null
                })
                .ToListAsync();
        }

        // HELPER METHOD (VERY IMPORTANT)
        private async Task<byte[]> ConvertToBytes(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}