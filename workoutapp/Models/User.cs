using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace workoutapp.Models
{
    public class User
    {
        public int UserId { get; set; }

        [StringLength(30)]
        public string? Username { get; set; }

        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "The email format is invalid")]
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
}
