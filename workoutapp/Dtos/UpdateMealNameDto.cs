using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class UpdateMealNameDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string MealName { get; set; }
    }
}
