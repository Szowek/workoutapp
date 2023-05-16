namespace workoutapp.Models
{
    public class DietMeal
    {
        public int DietMealId { get; set; }
        public int calendarDayId { get; set; }
        public int WhichMeal { get; set; }

        public virtual CalendarDay CalendarDay { get; set; }
        public virtual ProductOfMeal ProductOfMeal { get; set; }
    }
}
