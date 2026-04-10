using HostelManage.Data;
using HostelManage.Models;
using HostelManage.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using HostelManage.Application.DTOs.Feedback;

namespace HostelManage.Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly AppDbContext _context;

        public FeedbackService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> AddFeedback(FeedbackCreateDTO dto)
        {
            var feedback = new Feedback
            {
                HostelID = dto.HostelID,
                UserID = dto.UserID,
                Rating = dto.Rating,
                Comments = dto.Comments,
                FeedbackDate = DateTime.Now
            };

            _context.Feedback.Add(feedback);
            await _context.SaveChangesAsync();

            return feedback;
        }

        public async Task<int> GetFeedbackCount()
        {
            return await _context.Feedback.CountAsync();
        }

        public async Task<IEnumerable<object>> GetAllFeedbacks()
        {
            return await _context.Feedback
                .OrderByDescending(f => f.FeedbackDate)
                .ToListAsync();
        }

        public async Task<object> GetAverageRating(int hostelId)
        {
            var ratings = await _context.Feedback
                .Where(f => f.HostelID == hostelId)
                .ToListAsync();

            if (!ratings.Any())
                return new { average = (double?)null };

            var avg = ratings.Average(f => f.Rating);
            return new { average = avg };
        }

        public async Task<IEnumerable<object>> GetFeedbackByHostelId(int hostelId)
        {
            var feedbacks = await _context.Feedback
                .Where(f => f.HostelID == hostelId)
                .OrderByDescending(f => f.FeedbackDate)
                .Select(f => new
                {
                    f.FeedbackID,
                    f.HostelID,
                    f.UserID,
                    f.Rating,
                    f.Comments,
                    f.FeedbackDate,
                    Username = _context.User
                        .Where(u => u.UserID == f.UserID)
                        .Select(u => u.Name)
                        .FirstOrDefault()
                })
                .ToListAsync();

            if (!feedbacks.Any())
                throw new Exception($"No feedback found for Hostel ID {hostelId}");

            return feedbacks;
        }

        public async Task<string> DeleteFeedback(int feedbackId, int hostelId)
        {
            var feedback = await _context.Feedback.FindAsync(feedbackId);

            if (feedback == null)
                throw new Exception("Feedback not found");

            if (feedback.HostelID != hostelId)
                throw new Exception("Unauthorized");

            _context.Feedback.Remove(feedback);
            await _context.SaveChangesAsync();

            return "Feedback deleted successfully";
        }
    }
}