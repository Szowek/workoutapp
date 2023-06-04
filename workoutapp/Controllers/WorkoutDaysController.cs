using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using workoutapp.DAL;
using workoutapp.Dtos;
using workoutapp.Models;

namespace workoutapp.Controllers
{
    [Route("api/workoutplans/{workoutPlanId}")]
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
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateWorkoutDay(int workoutPlanId, [FromBody]WorkoutDayDto workoutDayDto)
        {

            int userID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userID);

            if (userID == 0)
            {
                return BadRequest();
            }

            var newWorkoutDay = new WorkoutDay
            {
                WorkoutPlanId = workoutPlanId
            };

            _context.WorkoutDays.Add(newWorkoutDay);
            _context.SaveChanges();

            return Ok("Stworzyles WorkoutDay");

            /*
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
            */
        }


        // Metoda usuwania WorkoutDaya dla danego WorkoutPlanu
        [HttpDelete("{workoutDayId}")]
        [Authorize]
        public async Task<IActionResult> DeleteWorkoutDay(int workoutPlanId, [FromRoute] int workoutDayId)
        {
            var workoutPlan = _context.WorkoutPlans.Include(wp => wp.WorkoutDays).FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan != null)
            {
                var workoutDay = workoutPlan.WorkoutDays.FirstOrDefault(wd => wd.WorkoutDayId == workoutDayId);
                if (workoutDay != null)
                {
                    _context.WorkoutDays.Remove(workoutDay);
                    _context.SaveChanges();
                    return Ok("Usunales WorkoutDay");
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

        // Metoda zwracająca wszystkie WorkoutDay dla WorkoutPlanu 
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllWorkoutDays(int workoutPlanId)
        {
            var workoutDays = _context.WorkoutDays.ToList();

            return Ok(workoutDays);
        }

        // Metoda zwracająca WorkoutDay na podstawie id dla WorkoutPlanu
        [HttpGet("{workoutDayId}")]
        public async Task<IActionResult> GetWorkoutDay(int workoutPlanId, [FromRoute] int workoutDayId)
        {
            int userID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userID);

            var workoutPlan = await _context.WorkoutPlans.Include(wp => wp.WorkoutDays).FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan != null)
            {
                var workoutDay = workoutPlan.WorkoutDays.FirstOrDefault(wd => wd.WorkoutDayId == workoutDayId);
                if (workoutDay != null)
                {
                    return Ok(new { UserId = userID, WorkoutPlanId = workoutPlanId, WorkoutDayId = workoutDayId });
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
