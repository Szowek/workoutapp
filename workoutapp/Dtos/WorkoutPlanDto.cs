using System.ComponentModel.DataAnnotations;
using workoutapp.Dtos;

namespace workoutapp.Models
{
    public class WorkoutPlanDto
    {
         public int WorkoutPlanId { get; set; }
        public string Name { get; set; }

        public bool? isPreferred { get; set; } = false;
        public uint DaysCount { get; set; }
        public virtual List<WorkoutDayDto> WorkoutDays { get; set; }

        public virtual List<NoteDto> Notes { get; set; }

    }
}
