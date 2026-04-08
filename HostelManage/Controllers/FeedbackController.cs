using Microsoft.AspNetCore.Mvc;
using HostelManage.Models;
using Microsoft.EntityFrameworkCore;
using HostelManage.Data;


namespace HostelManage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<IActionResult> PostFeedback([FromBody] Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            feedback.FeedbackDate = DateTime.Now;
            _context.Feedback.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeedbackByHostelID), new { HostelID = feedback.HostelID }, feedback);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetFeedbackCount()
        {
            var count = await _context.Feedback.CountAsync();
            return Ok(new { Count = count });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var feedbacks = await _context.Feedback
                .OrderByDescending(f => f.FeedbackDate)
                .ToListAsync();

            if (feedbacks == null || feedbacks.Count == 0)
            {
                return NotFound("No feedbacks found.");
            }

            return Ok(feedbacks);
        }
        [HttpGet("average/{HostelID}")]
        public async Task<IActionResult> GetAverageRating(int HostelID)
        {
            var ratings = await _context.Feedback
                .Where(f => f.HostelID == HostelID)
                .ToListAsync();

            if (!ratings.Any())
                return Ok(new { average = (double?)null });

            var average = ratings.Average(f => f.Rating);
            return Ok(new { average });
        }


        // GET: api/Feedback/Hostel/{HostelId}
        [HttpGet("Hostel/{HostelID}")]
        public async Task<IActionResult> GetFeedbackByHostelID(int HostelID)
        {
            var feedbacks = await _context.Feedback
                .Where(f => f.HostelID == HostelID)
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
                        .Select(u => u.Name) // or .Username if that’s the field name
                        .FirstOrDefault()
                })
                .ToListAsync();

            if (feedbacks == null || feedbacks.Count == 0)
            {
                return NotFound($"No feedback found for Hostel ID {HostelID}.");
            }

            return Ok(feedbacks);
        }

        // DELETE: api/Feedback/{feedbackId}?hostelId=123
        [HttpDelete("{feedbackId}")]
        public async Task<IActionResult> DeleteFeedback(int feedbackId, [FromQuery] int hostelId)
        {
            var feedback = await _context.Feedback.FindAsync(feedbackId);

            if (feedback == null)
            {
                return NotFound(new { message = "Feedback not found." });
            }

            if (feedback.HostelID != hostelId)
            {
                return Unauthorized(new { message = "You are not authorized to delete this feedback." });
            }

            _context.Feedback.Remove(feedback);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Feedback deleted successfully." });
        }

    }
}