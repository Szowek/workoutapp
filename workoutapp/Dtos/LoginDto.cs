using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class LoginDto
    {
        [StringLength(30)]
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
