using System.ComponentModel.DataAnnotations;

namespace workoutapp.Models
{
    public class UserExercise
    {
        public int ExerciseId { get; set; }

        [Required]
        public string ExerciseName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string BodyPart { get; set; }

        [Required]
        public int NumberOfSeries { get; set; }

        [Required]
        public int NumberOfRepeats { get; set; }

        public int CreatedById { get; set; }

        public virtual User CreatedBy { get; set; }

        public int WorkoutDayId { get; set; }

        public virtual WorkoutPlan WorkoutPlan { get; set; }
    }
}
