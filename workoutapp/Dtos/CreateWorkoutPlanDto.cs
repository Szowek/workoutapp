namespace workoutapp.Dtos
{
    public class CreateWorkoutPlanDto
    {
        public int WorkoutPlanId { get; set; }

        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
