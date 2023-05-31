using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using workoutapp.DAL;
using workoutapp.Models;

namespace workoutapp.Controllers
{
    [Route("api/workoutplans/workoutdays")]
    //[Route("api/workoutplans/{workoutPlanId}/workoutdays")] ????????????????????????????????
    [ApiController]
    public class WorkoutDaysController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutDaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Metoda tworzenia WorkoutDaya dla danego WorkoutPlanu
        [HttpPost]
        public async Task<IActionResult> CreateWorkoutDay(int workoutPlanId, [FromBody] WorkoutDay workoutDay)
        {
            var workoutPlan = _context.WorkoutPlans.Include(wp => wp.WorkoutDays).FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);
            if (workoutPlan != null)
            {
                workoutDay.WorkoutPlanId = workoutPlan.WorkoutPlanId; // Przypisanie klucza obcego
                workoutPlan.WorkoutDays.Add(workoutDay);
                _context.SaveChanges();

                var id = workoutPlan.WorkoutPlanId;

                return Created($"/api/workoutplans/workoutdays/{id}", null);

            }
            else
            {
                return NotFound();
            }
        }


        // Metoda usuwania WorkoutDaya dla danego WorkoutPlanu
        [HttpDelete("workoutplans/{workoutPlanId}/workoutdays/{workoutDayId}")]
        public async Task<IActionResult> DeleteWorkoutDay(int workoutPlanId, int workoutDayId)
        {
            var workoutPlan = _context.WorkoutPlans.Include(wp => wp.WorkoutDays).FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);
            if (workoutPlan != null)
            {
                var workoutDay = workoutPlan.WorkoutDays.FirstOrDefault(wd => wd.WorkoutDayId == workoutDayId);
                if (workoutDay != null)
                {
                    _context.WorkoutDays.Remove(workoutDay);
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
