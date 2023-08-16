using System.ComponentModel.DataAnnotations;

namespace workoutapp.Models
{
    public class CalendarDay
    {
        public int CalendarDayId { get; set; }

        [Required]
        public string CalendarDate { get; set; }
        public int CalendarId { get; set; }
        public int WorkoutDayId { get; set; }
        public virtual Calendar Calendar { get; set; }
        public virtual WorkoutDay WorkoutDay { get; set; }
    }
}
