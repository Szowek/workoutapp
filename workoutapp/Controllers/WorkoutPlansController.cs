using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using workoutapp.DAL;
using workoutapp.Models;

namespace workoutapp.Controllers
{

    [Route("api/workoutplans")]
    [ApiController]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutPlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Metoda tworzenia WorkoutPlanu dla danego użytkownika
        [HttpPost]
        public async Task<IActionResult> CreateWorkoutPlan(int userId, [FromBody] WorkoutPlan workoutPlan)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {

                workoutPlan.UserId = user.UserId; // Przypisanie klucza obcego
                user.WorkoutPlans.Add(workoutPlan);
                _context.SaveChanges();

                var id = workoutPlan.WorkoutPlanId;

                return Created($"/api/workoutplans/{id}", null);

            }
            else
            {
                return NotFound();
            }

        }

        // Metoda usuwania WorkoutPlanu dla danego użytkownika
        [HttpDelete("{workoutPlanId}")]
        public async Task<IActionResult> DeleteWorkoutPlan(int userId, int workoutPlanId)
        {
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                var workoutPlan = user.WorkoutPlans.FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);
                if (workoutPlan != null)
                {
                    _context.WorkoutPlans.Remove(workoutPlan);
                    _context.SaveChanges();
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

    }

}
