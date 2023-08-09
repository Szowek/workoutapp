namespace workoutapp.Models
{
    public class WorkoutPlan
    {
        public int WorkoutPlanId { get; set; }

        public string? Name { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual List<WorkoutDay> WorkoutDays { get; set; }


    }
}
