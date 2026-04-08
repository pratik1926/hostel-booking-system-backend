using HostelManage.Models;


namespace HostelManage.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdmin(string username, string password);
        Task<int> GetHostelCount();
        Task<List<Hostel>> GetHostels();
        Task<Hostel> GetHostelById(int id);
        Task<int> GetUserCount();
        Task<List<User>> GetUsers();
        Task SaveChanges();
    }
}
