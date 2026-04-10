using HostelManage.Application.DTOs.User;

namespace HostelManage.Application.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateUser(UserCreateDTO dto);
        Task<object> Login(UserLoginDTO dto);
        Task<string> ResetPassword(UserResetPasswordDTO dto);
        Task<object> GetUserDetails(int userId);
        Task<string> EditProfile(int userId, UserEditDTO dto);
        Task<object> GetHostels();
        Task<object> GetHostelRooms(int hostelId);
    }
}
