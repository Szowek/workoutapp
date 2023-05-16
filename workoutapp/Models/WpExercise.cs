namespace workoutapp.Models
{
    public class WpExercise
    {
        public int wdId { get; set; }
        public int ExerciseId { get; set; }

        public virtual WorkoutDay WorkoutDay { get; set; }

        public virtual List<Exercise> Exercises { get; set; }
    }
}
