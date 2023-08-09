namespace workoutapp.Models
{
    public class WorkoutPlanDto
    {
        public int WorkoutPlanId { get; set; }
       // public int UserId { get; set; }
        public string Name { get; set; }

        public virtual List<WorkoutDay> WorkoutDays { get; set; }

    }
}
