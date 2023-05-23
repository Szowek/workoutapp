using Microsoft.EntityFrameworkCore;
using workoutapp.Models;

namespace workoutapp.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        }
        public DbSet<User> Users { get; set; }
    }
}
