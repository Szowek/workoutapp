using System.ComponentModel.DataAnnotations;
using workoutapp.Models;

namespace workoutapp.Dtos
{
    public class CreateMealDto
    {
        [Required]
        public string MealName { get; set; }

        public int CalendarDayId { get; set; }
    }
}
