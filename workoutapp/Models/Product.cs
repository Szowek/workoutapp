namespace workoutapp.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public virtual ProductOfMeal ProductOfMeal { get; set; }

        public virtual List<ProductCategory> ProductCategories { get; set; }
    }
}
