using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductKcal { get; set; }
        public string ProductCategoryId { get; set; }
    }
}
