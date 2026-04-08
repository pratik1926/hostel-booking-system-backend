using Microsoft.EntityFrameworkCore;
using HostelManage.Data;
using HostelManage.Models;
using HostelManage.Repositories.Interfaces;

namespace HostelManage.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Admin> GetAdmin(string username, string password)
        {
            return await _context.Admin
                .FirstOrDefaultAsync(a => a.Username == username && a.Password == password);
        }

        public async Task<int> GetHostelCount()
            => await _context.Hostel.CountAsync();

        public async Task<List<Hostel>> GetHostels()
            => await _context.Hostel.ToListAsync();

        public async Task<Hostel> GetHostelById(int id)
            => await _context.Hostel.FindAsync(id);

        public async Task<int> GetUserCount()
            => await _context.User.CountAsync();

        public async Task<List<User>> GetUsers()
            => await _context.User.ToListAsync();

        public async Task SaveChanges()
            => await _context.SaveChangesAsync();
    }
}