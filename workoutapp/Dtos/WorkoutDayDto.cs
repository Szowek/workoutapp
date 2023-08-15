using workoutapp.Models;

namespace workoutapp.Dtos
{
    public class WorkoutDayDto
    {
        //public int WorkoutDayId { get; set; }

        public virtual List<UserExercise> UserExercises { get; set; }
    }
}
