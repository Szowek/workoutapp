using System.ComponentModel.DataAnnotations;

namespace workoutapp.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }

        [Required(ErrorMessage = "Enter a description for exercise")]
        [StringLength(50)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Enter a name for exercise")]
        [StringLength(50)]
        public string Name { get; set; }
        public int PullOrPush { get; set; }
        public int BodyPart { get; set; }

        public virtual WpExercise WpExercise { get; set; }

    }
}
