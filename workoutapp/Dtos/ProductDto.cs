using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductKcal { get; set; }
        public int ProductWeight { get; set; }
        public float ProductProtein { get; set; }

        public float ProductFat { get; set; }

        public float ProductCarbs { get; set; }
        public string ProductCategoryId { get; set; }

        public string ProductCategoryName { get; set; }
    }
}
