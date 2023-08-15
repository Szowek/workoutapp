namespace workoutapp.Dtos
{
    public class CreateWorkoutPlanDto
    {
        public string Name { get; set; }
        public bool? isPreferred { get; set; } = false;
        public int UserId { get; set; }
    }
}
