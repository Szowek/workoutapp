using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class UpdateNoteDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string NoteName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Text { get; set; }
    }
}
