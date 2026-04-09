using HostelManage.Data;
using HostelManage.Models;
using HostelManage.Application.DTOs;
using HostelManage.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HostelManage.Application.Services
{
    public class HostelDescriptionService : IHostelDescriptionService
    {
        private readonly AppDbContext _context;

        public HostelDescriptionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> AddDescription(HostelDescriptionCreateDTO dto)
        {
            var hostelExists = await _context.Hostel.FindAsync(dto.HostelID);

            if (hostelExists == null)
                throw new Exception($"Hostel with ID {dto.HostelID} does not exist");

            var model = new HostelDescription
            {
                HostelID = dto.HostelID,
                Location = dto.Location,
                Description = dto.Description,
                RoomType1Count = dto.RoomType1Count,
                RoomType2Count = dto.RoomType2Count,
                RoomType3Count = dto.RoomType3Count,
                RoomType4Count = dto.RoomType4Count,
                CreatedDate = DateTime.Now
            };

            _context.HostelDescription.Add(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<object> GetByHostelId(int hostelId)
        {
            var description = await _context.HostelDescription
                .FirstOrDefaultAsync(h => h.HostelID == hostelId);

            if (description == null)
                throw new Exception($"No description found for HostelID {hostelId}");

            return description;
        }

        public async Task<string> UpdateDescription(int hostelId, HostelDescriptionUpdateDTO dto)
        {
            if (hostelId != dto.HostelID)
                throw new Exception("HostelID mismatch");

            var existing = await _context.HostelDescription
                .FirstOrDefaultAsync(h => h.HostelID == hostelId);

            if (existing == null)
                throw new Exception($"No description found for HostelID {hostelId}");

            existing.Location = dto.Location;
            existing.Description = dto.Description;
            existing.RoomType1Count = dto.RoomType1Count;
            existing.RoomType2Count = dto.RoomType2Count;
            existing.RoomType3Count = dto.RoomType3Count;
            existing.RoomType4Count = dto.RoomType4Count;

            await _context.SaveChangesAsync();

            return "Hostel description updated successfully";
        }
    }
}