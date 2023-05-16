namespace workoutapp.Models
{
    public class ProductOfMeal
    {
        public int DietMealId { get; set; }
        public int ProductId { get; set; }

        public virtual DietMeal DietMeal { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
