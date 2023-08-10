﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace workoutapp.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }

        [Required]
        public string ExerciseName { get; set;}

        [Required]
        public string Description { get; set;}

        [Required]
        public string BodyPart { get; set;}

        public int? NumberOfSeries { get; set; }

        public int? NumberOfRepeats { get; set;}

        public int WorkoutDayId { get; set; }

        public virtual WorkoutDay WorkoutDay { get; set; }

    }
}
