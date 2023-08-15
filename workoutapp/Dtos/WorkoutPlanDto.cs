namespace workoutapp.Models
{
    public class WorkoutPlanDto
    {
       // public int WorkoutPlanId { get; set; }
        public string Name { get; set; }

        public bool? isPreferred { get; set; } = false;
        public virtual List<WorkoutDay> WorkoutDays { get; set; }

        public virtual List<Note> Notes { get; set; }

    }
}
