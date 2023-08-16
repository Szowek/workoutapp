using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class CreateWorkoutDayDto
    {
        public int WorkoutPlanId { get; set; }
        public string CalendarDate { get; set; }

    }
}
