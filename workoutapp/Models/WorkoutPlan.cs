namespace workoutapp.Models
{
    public class WorkoutPlan
    {
        public int WorkoutPlanId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public List<WorkoutDay> WorkoutDays { get; set; }


    }
}
