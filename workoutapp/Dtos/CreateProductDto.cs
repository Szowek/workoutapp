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
        public int ProductWeight { get; set; }
            
        public float ProductCarbs { get; set; }
        
        public float ProductFat { get; set; }

        public float ProductProtein { get; set; }

        [Required]
        public string ProductCategoryName { get; set; }
        public int MealId { get; set; }
    }
}
