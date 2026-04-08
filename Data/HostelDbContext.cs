using Microsoft.EntityFrameworkCore;
using FYP_Backend.Model;


namespace FYP_Backend.Data
{
    public class HostelDbContext : DbContext
    {
        public HostelDbContext(DbContextOptions<HostelDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
