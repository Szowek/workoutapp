namespace workoutapp.Models
{
    public class WorkoutDay
    {
        public int wpId { get; set; }
        public int wdId { get; set; }

        public virtual WorkoutPlan WorkoutPlan { get; set; }
        public virtual CalendarDay CalendarDay { get; set; }
    }
}
