namespace workoutapp.Models
{
    public class CalendarDay
    {
        public int CalendarDayId { get; set; }
        public int CalendarId { get; set; }
        public int wdId { get; set; }

        public virtual Calendar Calendar { get; set; }
        public virtual WorkoutDay WorkoutDay { get; set; }

        public virtual List<DietMeal> DietMeals { get; set; }
    }
}
