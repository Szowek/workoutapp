namespace workoutapp.Models
{
    public class Calendar
    {
        public int CalendarId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual CalendarDay CalendarDay { get; set; }
    }
}
