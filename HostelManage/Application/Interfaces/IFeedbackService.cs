using HostelManage.Models;
using HostelManage.Application.DTOs;


namespace HostelManage.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<object> AddFeedback(FeedbackCreateDTO dTO);
        Task<int> GetFeedbackCount();
        Task<IEnumerable<object>> GetAllFeedbacks();
        Task<object> GetAverageRating(int hostelId);
        Task<IEnumerable<object>> GetFeedbackByHostelId(int hostelId);
        Task<string> DeleteFeedback(int feedbackId, int hostelId);

    }
}
