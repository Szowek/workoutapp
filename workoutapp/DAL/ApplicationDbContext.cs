using Microsoft.EntityFrameworkCore;
using workoutapp.Models;

namespace workoutapp.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutDay> WorkoutDays { get; set; }

        public DbSet<Exercise> Exercises { get; set; }
    }
}
