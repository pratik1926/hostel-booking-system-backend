using HostelManage.Application.DTOs.Hostel;

namespace HostelManage.Application.Interfaces
{
    public interface IHostelService
    {
        Task<string> CreateHostel(HostelCreateDTO request);
        Task<object> Login(LoginDTO request);
        Task<string> ResetPassword(HostelResetPasswordDTO request);
        Task<string> EditProfile(int hostelId, HostelEditDTO request);
        Task<object> GetHostelById(int id);
    }
}