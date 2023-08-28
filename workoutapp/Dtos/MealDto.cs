using workoutapp.Models;

namespace workoutapp.Dtos
{
    public class MealDto
    {
        public int MealId { get; set; }
        public string MealName { get; set; }
        public int TotalKcal { get; set; } = 0;

        public float TotalProtein { get; set; } = 0;

        public float TotalFat { get; set; } = 0;

        public float TotalCarbs { get; set; } = 0;

        public virtual List<ProductDto> Products { get; set; }

    }
}
