using HostelManage.Application.Interfaces;
using HostelManage.Repositories.Interfaces;

namespace HostelManage.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repo;

        public AdminService(IAdminRepository repo)
        {
            _repo = repo;
        }

        public async Task<(bool Success, string Message)> Login(string username, string password)
        {
            var admin = await _repo.GetAdmin(username, password);

            if (admin == null)
                return (false, "Invalid username or password.");

            return (true, "Login successful!");
        }

        public async Task<int> GetHostelCount()
            => await _repo.GetHostelCount();

        public async Task<object> GetHostels()
        {
            var hostels = await _repo.GetHostels();

            return hostels.Select(h => new
            {
                h.HostelID,
                h.HostelName,
                h.Address,
                h.Status
            });
        }

        public async Task<(bool Success, string Message)> UpdateHostelStatus(int id, bool status)
        {
            var hostel = await _repo.GetHostelById(id);

            if (hostel == null)
                return (false, "Hostel not found.");

            hostel.Status = status;
            await _repo.SaveChanges();

            return (true, "Hostel status updated successfully.");
        }

        public async Task<int> GetUserCount()
            => await _repo.GetUserCount();

        public async Task<object> GetUsers()
        {
            var users = await _repo.GetUsers();

            return users.Select(u => new
            {
                u.UserID,
                u.Name,
                u.Email
            });
        }
    }
}