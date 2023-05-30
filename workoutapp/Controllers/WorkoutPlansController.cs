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
                //ss
            }
            else
            {
                return NotFound();
            }

        }

        // Metoda usuwania WorkoutPlanu dla danego użytkownika
        [HttpDelete("{workoutPlanId}")]
        public async Task<IActionResult> DeleteWorkoutPlan(int userId, [FromRoute] int workoutPlanId)
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


        // Metoda zwracająca wszystkie WorkoutPlany użytkownika z listy
        [HttpGet]
        public async Task<IActionResult> GetAllWorkoutPlans(int userId)
        {
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                var workoutPlans = user.WorkoutPlans.ToList();
                return Ok(workoutPlans);
            }
            else
            {
                return NotFound();
            }
        }

        // Metoda zwracająca WorkoutPlan użytkownika na podstawie id
        [HttpGet("{workoutPlanId}")]
        public async Task<IActionResult> GetWorkoutPlanById(int userId, [FromRoute] int workoutPlanId)
        {
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                var workoutPlan = user.WorkoutPlans.FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);
                if (workoutPlan != null)
                {
                    return Ok(workoutPlan);
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
