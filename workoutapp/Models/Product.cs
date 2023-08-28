using System.ComponentModel.DataAnnotations;

namespace workoutapp.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int ProductKcal { get; set; }

        public int ProductWeight { get; set; }
        public float ProductProtein { get; set; }
        public float ProductCarbs { get; set; }

        public float ProductFat { get; set; }
        public int ProductCategoryId { get; set; }

        [Required]
        public string ProductCategoryName { get; set; }
        public int MealId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

        public virtual Meal Meal { get; set; }

    }
}
