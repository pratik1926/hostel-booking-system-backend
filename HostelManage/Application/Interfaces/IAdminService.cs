using System.Threading.Tasks;

namespace HostelManage.Application.Interfaces
{
    public interface IAdminService
    {
        Task<(bool Success, string Message)> Login(string username, string password);
        Task<int> GetHostelCount();
        Task<object> GetHostels();
        Task<(bool Success, string Message)> UpdateHostelStatus(int id, bool status);
        Task<int> GetUserCount();
        Task<object> GetUsers();
    }
}