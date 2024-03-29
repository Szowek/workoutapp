﻿using System.ComponentModel.DataAnnotations;
using workoutapp.Models;

namespace workoutapp.Dtos
{
    public class CreateMealDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string MealName { get; set; }

        public int CalendarDayId { get; set; }
    }
}
