using System.ComponentModel.DataAnnotations;

namespace workoutapp.Dtos
{
    public class UpdateUserPasswordDto
    {
        [Required]
        public string Password { get; set; }
    }
}
