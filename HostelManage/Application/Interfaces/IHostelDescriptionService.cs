using HostelManage.Application.DTOs;

namespace HostelManage.Application.Interfaces
{
    public interface IHostelDescriptionService
    {
        Task<object> AddDescription(HostelDescriptionCreateDTO dto);
        Task<object> GetByHostelId(int hostelId);
        Task<string> UpdateDescription(int hostelId, HostelDescriptionUpdateDTO dto);
    }
}