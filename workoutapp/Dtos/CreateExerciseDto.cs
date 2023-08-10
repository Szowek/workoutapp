using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class CreateExerciseDto
    {
        [Required]
        public string ExerciseName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string BodyPart { get; set; }

        public int? NumberOfSeries { get; set; }

        public int? NumberOfRepeats { get; set; }

        public int WorkoutDayId { get; set; }
    }
}
