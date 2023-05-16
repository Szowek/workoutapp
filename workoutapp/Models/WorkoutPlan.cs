namespace workoutapp.Models
{
    public class WorkoutPlan
    {
        public int UserId { get; set; }
        public int wpId { get; set; }

        public virtual User User { get; set; }
        public virtual WorkoutDay WorkoutDay { get; set; }
    }
}
