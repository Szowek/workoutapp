namespace workoutapp.Models
{
    public class WorkoutDay
    {
        public int WorkoutDayId { get; set; }

        public int WorkoutPlanId { get; set; }
        public virtual WorkoutPlan WorkoutPlan { get; set; }

        public virtual List<Exercise> Exercises { get; set; }
    }
}
