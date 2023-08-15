using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class CreateNoteDto
    {
        public string NoteName { get; set; }

        [Required]
        public string Text { get; set; }
        public int WorkoutPlanId { get; set; }
    }
}
