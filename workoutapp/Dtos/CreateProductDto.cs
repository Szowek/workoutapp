using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class CreateProductDto
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public int ProductKcal { get; set; }

        [Required]
        public string ProductCategoryName { get; set; }
        public int MealId { get; set; }
    }
}
