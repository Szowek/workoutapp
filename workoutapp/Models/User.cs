using System.ComponentModel.DataAnnotations;

namespace workoutapp.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Enter your username")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "The email format is invalid")]
        public string email { get; set; }

        public virtual WorkoutPlan WorkoutPlan { get; set; }
        public virtual UserData UserData { get; set; }
        public virtual Calendar Calendar { get; set; }
    }
}
