namespace workoutapp.Models
{
    public class ProductCategory
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public virtual Product Product { get; set; }
    }
}
