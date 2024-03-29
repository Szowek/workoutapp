﻿using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class CreateWorkoutPlanDto
    {
        [Required]
        public string Name { get; set; }
        public bool? isPreferred { get; set; } = false;
        public int UserId { get; set; }
        [Required]
        public uint DaysCount { get; set; }
    }
}
