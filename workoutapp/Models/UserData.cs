using System.ComponentModel.DataAnnotations;

namespace workoutapp.Models
{
    public class UserData
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [StringLength(50)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "The email format is invalid")]
        public string Email { get; set; }

        public virtual User User { get; set; }
    }
}
