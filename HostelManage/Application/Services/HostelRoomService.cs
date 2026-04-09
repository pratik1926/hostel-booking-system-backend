using HostelManage.Data;
using HostelManage.Models;
using HostelManage.Application.DTOs;
using HostelManage.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HostelManage.Application.Services
{
    public class HostelRoomService : IHostelRoomService
    {
        private readonly AppDbContext _context;

        public HostelRoomService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> UploadRoomImages(int hostelId, HostelRoomUploadDTO dto)
        {
            var hostelExists = await _context.Hostel.FindAsync(hostelId);

            if (hostelExists == null)
                throw new Exception("Hostel not found");

            var room = new HostelRoom
            {
                HostelID = hostelId,
                RoomImage1 = await ConvertToBytes(dto.RoomImage1),
                RoomImage2 = await ConvertToBytes(dto.RoomImage2),
                RoomImage3 = await ConvertToBytes(dto.RoomImage3),
                RoomImage4 = await ConvertToBytes(dto.RoomImage4)
            };

            _context.HostelRoom.Add(room);
            await _context.SaveChangesAsync();

            return new { message = "Room added successfully", room.RoomID };
        }

        public async Task<object> GetRoomsByHostelId(int hostelId)
        {
            var rooms = await _context.HostelRoom
                .Where(r => r.HostelID == hostelId)
                .ToListAsync();

            if (!rooms.Any())
                throw new Exception("No rooms found");

            return rooms.Select(r => new
            {
                r.RoomID,
                r.HostelID,
                RoomImage1 = r.RoomImage1 != null ? Convert.ToBase64String(r.RoomImage1) : null,
                RoomImage2 = r.RoomImage2 != null ? Convert.ToBase64String(r.RoomImage2) : null,
                RoomImage3 = r.RoomImage3 != null ? Convert.ToBase64String(r.RoomImage3) : null,
                RoomImage4 = r.RoomImage4 != null ? Convert.ToBase64String(r.RoomImage4) : null
            });
        }

        public async Task<string> DeleteRoom(int roomId)
        {
            var room = await _context.HostelRoom.FindAsync(roomId);

            if (room == null)
                throw new Exception("Room not found");

            _context.HostelRoom.Remove(room);
            await _context.SaveChangesAsync();

            return "Room deleted successfully";
        }

        public async Task<string> UpdateAllRoomImages(int hostelId, HostelRoomUploadDTO dto)
        {
            var rooms = await _context.HostelRoom
                .Where(r => r.HostelID == hostelId)
                .ToListAsync();

            if (!rooms.Any())
                throw new Exception("No rooms found");

            foreach (var room in rooms)
            {
                if (dto.RoomImage1 != null) room.RoomImage1 = await ConvertToBytes(dto.RoomImage1);
                if (dto.RoomImage2 != null) room.RoomImage2 = await ConvertToBytes(dto.RoomImage2);
                if (dto.RoomImage3 != null) room.RoomImage3 = await ConvertToBytes(dto.RoomImage3);
                if (dto.RoomImage4 != null) room.RoomImage4 = await ConvertToBytes(dto.RoomImage4);
            }

            await _context.SaveChangesAsync();

            return "Images updated successfully";
        }

        private async Task<byte[]?> ConvertToBytes(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}