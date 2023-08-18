﻿using System.ComponentModel.DataAnnotations;

namespace workoutapp.Models
{
    public class Meal
    {
        public int MealId { get; set; }

        [Required]
        public string MealName { get; set; }

        public int TotalKcal { get; set; } = 0;

        public int CalendarDayId { get; set; }

        public virtual CalendarDay CalendarDay { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}