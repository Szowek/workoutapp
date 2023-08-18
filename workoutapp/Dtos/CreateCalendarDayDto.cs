using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class CreateCalendarDayDto
    {
        public int CalendarId { get; set; }

        [Required]
        public string CalendarDate { get; set; }

    }
}
