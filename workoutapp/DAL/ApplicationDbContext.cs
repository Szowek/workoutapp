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
        public DbSet<UserExercise> UserExercises { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<CalendarDay> CalendarDays { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Exercise>().HasData(

               new Exercise()
               {
                   ExerciseId = 1,
                   ExerciseName = "Pull ups",
                   Description = "Pull-ups are a bodyweight exercise where you grip an overhead bar with palms facing away. By pulling your body upward, " +
                    "you target your upper back muscles, especially the latissimus dorsi. This exercise also engages your biceps and core, " +
                    "providing a well-rounded upper body workout.",
                   BodyPart = "Shoulders"
               },
                new Exercise()
                {
                    ExerciseId = 2,
                    ExerciseName = "Pin bench press",
                    Description = "Pin bench press is a weightlifting move. Barbell starts on safety pins, " +
                    "adding dead-stop challenge. Lift and press for chest, triceps, and shoulder strength. Targets bench press weak points.",
                    BodyPart = "Chest"
                },
                new Exercise()
                {
                    ExerciseId = 3,
                    ExerciseName = "Face pull",
                    Description = "Face pull is an exercise using a cable machine and rope attachment. " +
                    "Pull the rope towards your face, keeping elbows high, to work rear deltoids and upper back. Enhances shoulder stability and posture.",
                    BodyPart = "Shoulders"
                },
                new Exercise()
                {
                    ExerciseId = 4,
                    ExerciseName = "Skull crusher pullover",
                    Description = "Skull Crusher Pullover combines two exercises. Begin with a weightlifting barbell, lower it to your forehead (skull crusher), " +
                    "then move it over and behind your head (pullover). This engages triceps, chest, and lats for a comprehensive upper body workout.",
                    BodyPart = "Triceps"

                },
                new Exercise()
                {
                    ExerciseId = 5,
                    ExerciseName = "Barbell biceps curl",
                    Description = "Barbell biceps curl is a classic exercise for building arm strength. " +
                    "Hold a barbell with an underhand grip, palms facing up, and curl it upward using your biceps. " +
                    "Lower the barbell with control. This targets the biceps muscles, helping to develop arm definition and strength.",
                    BodyPart = "Biceps"
                },
                  new Exercise()
                  {
                      ExerciseId = 6,
                      ExerciseName = "Swiss ball plank",
                      Description = "The Swiss ball plank is an effective core exercise. Start in a plank position with your forearms on a Swiss ball and toes on the ground. " +
                    "Maintain a straight line from head to heels, engaging your core and stabilizing muscles. " +
                    "This exercise helps improve core strength, stability, and balance.\"",
                      BodyPart = "Core"

                  },
                new Exercise()
                {
                    ExerciseId = 7,
                    ExerciseName = "Standing barbell calf raise",
                    Description = "The standing barbell calf raise is a calf-strengthening exercise. Stand upright with a barbell resting on your upper back. " +
                    "Rise onto your toes by pushing through the balls of your feet, " +
                    "lifting your heels as high as possible. Lower your heels back down for a full range of motion. " +
                    "This exercise targets the calf muscles, enhancing lower leg strength and definition.",
                    BodyPart = "Calf"
                },
                new Exercise()
                {
                    ExerciseId = 8,
                    ExerciseName = "Reverse lunges with barbell",
                    Description = "Reverse lunges with a barbell are a lower body exercise. Hold a barbell on your upper back, step back with one leg into a lunge, " +
                    "lowering your back knee toward the ground." +
                    " Push through your front heel to return to the standing position. This targets the quadriceps, hamstrings, and glutes, enhancing leg strength and stability.",
                    BodyPart = "Quadriceps thighs "
                },
                new Exercise()
                {
                    ExerciseId = 9,
                    ExerciseName = "Elevated deadlift",
                    Description = "The elevated deadlift is a weightlifting exercise that involves lifting a barbell from an elevated platform. " +
                    "Stand on weight plates or blocks, grasp the barbell with an overhand grip, and lift by extending your hips and knees. Lower the barbell back down with control. " +
                    "This exercise targets the hamstrings, glutes, lower back, and core, promoting overall strength and muscle development.",
                    BodyPart = "Lower back"
                },
                new Exercise()
                {
                    ExerciseId = 10,
                    ExerciseName = "Dumbbell supinated biceps curls",
                    Description = "Dumbbell supinated biceps curls are a biceps-strengthening exercise. Hold dumbbells with palms facing up (supinated grip) and curl them upward while contracting your biceps. " +
                    "Lower the dumbbells with control. This exercise effectively isolates and builds the biceps muscles, enhancing arm strength and definition.",
                    BodyPart = "Biceps"
                });
        }
        
        
    }
}
