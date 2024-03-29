﻿using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class CreateWorkoutDayDto
    {
        public int WorkoutPlanId { get; set; }

        [Required]
        public string CalendarDate { get; set; }

        public int CalendarDayId { get; set; }

    }
}
